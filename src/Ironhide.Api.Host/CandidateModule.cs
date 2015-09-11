using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nancy;
using Nancy.ModelBinding;
using RestSharp;

namespace Ironhide.Api.Host
{
    public class StaticDictionary : IEnglishDictionary
    {
        readonly string[] words =
        {
            "cats", "rule", "dogs", "drool", "clean", "code", "materials", "needed", "this", "is", "hard",
            "what", "are", "you", "smoking", "shot", "gun", "down", "river", "super", "man"
        };

        public bool IsEnglishWord(string word)
        {
            return words.Select(x => x.ToLower()).Contains(word.Trim().ToLower());
        }
    }

    public class CandidateModule : NancyModule
    {
        const int MaxWords = 10;
        const decimal RequiredSuccessCountForWin = 20;
        const double MillisecondsForWin = 10000;

        static readonly List<GetValueRequests> GetValueRequests = new List<GetValueRequests>();

        static readonly string[] AllWords =
        {
            "ashuph", "goohee", "nygnas", "sherga", "cowhyw", "eerdox", "lekops", "gledsy",
            "iwefty", "yjyltu", "olapev", "ooptyn", "oansol", "xezyms", "eeping", "suckyj",
            "divorce", "frequent", "drown", "sharp", "blushing", "communication", "decoder", "internal", "column",
            "agreeable", "catsruledogsdrool", "cleancode", "materialsneeded", "thisishard", "whatareyousmoking",
            "pharaoh", "femur", "bird", "frightening", "bat", "gum", "gobbling", "diplomatic", "downriver",
            "healthy", "liquid", "flush", "after", "insurance", "beam", "harm", "shotgun", "business", "coal"
        };

        static readonly List<CandidateSuccess> CandidateSuccesses = new List<CandidateSuccess>();
        static readonly List<Winner> Winners = new List<Winner>();
        readonly Base64StringEncoder _base64Encoder;
        readonly CapsAlternator _capsAlternator;
        readonly SuperSecretEncodingAlgorithm _encoder;
        readonly FibonacciGenerator _fibonacciGenerator;
        readonly IHiringTeamNotifier _notifier;

        public CandidateModule()
        {
            _capsAlternator = new CapsAlternator();
            _encoder = new SuperSecretEncodingAlgorithm(new VowelEncoder(new FibonacciGenerator()), new VowelShifter(),
                _capsAlternator, new AsciiValueDelimiterAdder(), new WordSplitter(new StaticDictionary()));
            _fibonacciGenerator = new FibonacciGenerator();
            _base64Encoder = new Base64StringEncoder();
            _notifier = new SlackHiringTeamNotifications();

            Get["values/{guid}"] = p => GetValues(p);
            Post["values/{guid}", true] = async (p, ctx) => await PostValue(p);
        }

        dynamic GetValues(dynamic p)
        {
            Guid guid = GetGuid((string) p.guid);
            var rnd = new Random();
            var words = new List<string>();
            for (int i = 0; i < MaxWords; i++)
            {
                words.Add(_capsAlternator.Alternate(AllWords).ToArray()[rnd.Next(0, AllWords.Length - 1)]);
            }
            double startingFibonacciNumber = GetRandomFibonacciStartingNumber();
            GetValueRequests.Add(new GetValueRequests(guid, words, Convert.ToInt64(startingFibonacciNumber)));
            return Response.AsJson(new {words, startingFibonacciNumber});
        }

        async Task<dynamic> PostValue(dynamic p)
        {
            try
            {
                Guid guid = GetGuid((string) p.guid, Allow.Duplicates);
                var reqBody = this.Bind<NewValueRequest>();

                string candidateEncoded = reqBody.EncodedValue;
                if (string.IsNullOrEmpty(candidateEncoded)) throw new ArgumentNullException("encodedValue");
                string emailAddress = reqBody.EmailAddress;
                if (string.IsNullOrEmpty(emailAddress)) throw new ArgumentNullException("emailAddress");
                string webhookUrl = reqBody.WebhookUrl;
                if (string.IsNullOrEmpty(webhookUrl)) throw new ArgumentNullException("webhookUrl");
                string name = reqBody.Name;
                if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");
                string repoUrl = reqBody.RepoUrl;
                if (string.IsNullOrEmpty(repoUrl)) throw new ArgumentNullException("repoUrl");

                GetValueRequests previousRequest = GetMatchingPreviousRequest(guid);
                VerifyMatchingEncodedString(previousRequest, candidateEncoded);
                AddSuccessToList(emailAddress, webhookUrl);

                int recentSuccesses = RecentSuccesses(emailAddress);
                bool isWinner = recentSuccesses >= RequiredSuccessCountForWin;
                var repeatWinner =
                    Winners.Any(x => x.EmailAddress == emailAddress && x.Time > DateTime.UtcNow.AddMinutes(-10));

                if (isWinner && !repeatWinner)
                {
                    var randomFibonacciStartingNumber = GetRandomFibonacciStartingNumber().ToString();
                    await SendWebhook(webhookUrl, randomFibonacciStartingNumber);
                    var winner = new Winner(emailAddress, name, repoUrl, DateTime.UtcNow);
                    await NotifyHiringTeam(winner, randomFibonacciStartingNumber);
                    Winners.Add(winner);
                }
                string message = repeatWinner ? "Please wait 10 minutes before winning again to get the webhook post." : isWinner ? WinMessage() : SingleSuccessMessage(recentSuccesses);
                string status = (isWinner ? PostAttempt.Winner : PostAttempt.Success).ToString();

                return Response.AsJson(new {status, message});
            }
            catch (CandidateRequestException ex)
            {
                return Response.AsJson(new {status = "CrashAndBurn", message = ex.Message}, HttpStatusCode.BadRequest);
            }
        }

        async Task NotifyHiringTeam(Winner winner, string secretCode)
        {
            await _notifier.Notify(winner.EmailAddress, winner.Name, winner.RepoUrl, secretCode);
        }

        double GetRandomFibonacciStartingNumber()
        {
            double[] numbers = _fibonacciGenerator.Generate(40).ToArray();
            var rnd = new Random();
            return numbers[rnd.Next(0, numbers.Count())];
        }

        void AddSuccessToList(string emailAddress, string webhookUrl)
        {
            CandidateSuccesses.Add(new CandidateSuccess(emailAddress, webhookUrl, DateTime.UtcNow));
        }

        void VerifyMatchingEncodedString(GetValueRequests previousRequest, string candidateEncoded)
        {
            string ourEncoded = _encoder.Encode(previousRequest.StartingFibonacciNumber, previousRequest.Words.ToArray());
            string b64Encoded = _base64Encoder.Encode(ourEncoded);
            bool candidateFailedToProvideCorrectEncodedString = candidateEncoded != b64Encoded;
            if (candidateFailedToProvideCorrectEncodedString) throw new CandidateRequestException();
        }

        static GetValueRequests GetMatchingPreviousRequest(Guid guid)
        {
            GetValueRequests previousRequest = GetValueRequests.FirstOrDefault(x => x.Guid == guid);
            if (previousRequest == null) throw new CandidateRequestException();
            return previousRequest;
        }

        static string WinMessage()
        {
            return "You win! You should have already received the secret phrase at your webhook url.";
        }

        static string SingleSuccessMessage(int recentSuccesses)
        {
            return string.Format(
                "Felicidades! You have had {0} successes in the last {1} milliseconds. {2} more to go!", recentSuccesses,
                MillisecondsForWin, RequiredSuccessCountForWin - recentSuccesses);
        }

        int RecentSuccesses(string emailAddress)
        {
            DateTime minimumWinTime = DateTime.UtcNow.AddMilliseconds(-1*MillisecondsForWin);
            IEnumerable<CandidateSuccess> recentSuccesses =
                CandidateSuccesses.Where(x => x.EmailAddress == emailAddress
                                              && x.Time >= minimumWinTime);

            return recentSuccesses.Count();
        }

        async Task SendWebhook(string webhookUrl, string randomFibonacciStartingNumber)
        {
            var client = new RestClient(webhookUrl);
            var restRequest = new RestRequest {RequestFormat = DataFormat.Json};
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddBody(new
                                {
                                    secret = string.Format("{0} is the magic!", randomFibonacciStartingNumber)
                                });
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddHeader("Content-Type", "application/json");
            await client.ExecutePostTaskAsync(restRequest);
        }

        static Guid GetGuid(string guidString, Allow allow = Allow.OnlyUnique)
        {
            Guid guid;
            if (!Guid.TryParse(guidString, out guid)) throw new CandidateRequestException();
            if (allow == Allow.OnlyUnique && GetValueRequests.Any(x => x.Guid == guid))
                throw new CandidateRequestException();
            return guid;
        }
    }
}