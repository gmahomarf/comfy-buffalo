using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ironhide.Api.Host.Algorithms;
using Nancy;
using Nancy.ModelBinding;
using RestSharp;

namespace Ironhide.Api.Host
{
    public class CandidateModule : NancyModule
    {
        const int MaxWords = 10;
        const decimal RequiredSuccessCountForWin = 20;
        const double MillisecondsForWin = 10000;

        static readonly List<GetValueRequests> WordRequests = new List<GetValueRequests>();

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
        static readonly CapsAlternator CapsAlternator = new CapsAlternator();
        readonly Base64StringEncoder _base64Encoder = new Base64StringEncoder();

        readonly FibonacciGenerator _fibonacciGenerator = new FibonacciGenerator();

        readonly TheIncredibleHulkAlgorithm _theIncredibleHulkAlgorithm = new TheIncredibleHulkAlgorithm(new VowelShifter());

        readonly IronManAlgorithm _ironManAlgorithm = new IronManAlgorithm(new VowelShifter(), new AsciiValueDelimiterAdder());

        readonly IHiringTeamNotifier _notifier = new SlackHiringTeamNotifications();

        ThorAlgorithm _thorAlgorithm;

        public CandidateModule()
        {
            Get["values/{guid}"] = p => GetValues(p);

            Post["values/{guid}/thor"] = p => PostThor(p);
            Post["values/{guid}/theIncredibleHulk"] = p => PostHulk(p);
            Post["values/{guid}/ironMan"] = p => PostIronMan(p);
            Post["values/{guid}/captainAmerica"] = p => PostCap(p);

            Get["encoded/{guid}/thor"] = p => VerifyThor(p);
            Get["encoded/{guid}/captainAmerica"] = p => VerifyCap(p);
            Get["encoded/{guid}/ironMan"] = p => VerifyIronMan(p);
            Get["encoded/{guid}/theIncredibleHulk"] = p => VerifyHulk(p);
        }

        dynamic VerifyIronMan(dynamic p)
        {
            Guid guid = GetGuid((string) p.guid, Allow.Duplicates);
            return VerifyValue(_ironManAlgorithm, guid);
        }

        dynamic VerifyCap(dynamic p)
        {
            Guid guid = GetGuid((string) p.guid, Allow.Duplicates);
            return VerifyValue(GetCaptainAmericaAlgorithm(guid), guid);
        }

        static CaptainAmericaAlgorithm GetCaptainAmericaAlgorithm(Guid guid)
        {
            GetValueRequests previousRequest = GetMatchingPreviousRequest(guid);
            var captainAmericaAlgorithm = new CaptainAmericaAlgorithm(previousRequest.StartingFibonacciNumber,
                new VowelEncoder(new FibonacciGenerator()), new VowelShifter(),
                new AsciiValueDelimiterAdder());
            return captainAmericaAlgorithm;
        }

        dynamic VerifyHulk(dynamic p)
        {
            Guid guid = GetGuid((string) p.guid, Allow.Duplicates);
            return VerifyValue(_theIncredibleHulkAlgorithm, guid);
        }

        dynamic VerifyThor(dynamic p)
        {
            Guid guid = GetGuid((string) p.guid, Allow.Duplicates);
            GetValueRequests previousRequest = GetMatchingPreviousRequest(guid);

            _thorAlgorithm = new ThorAlgorithm(previousRequest.StartingFibonacciNumber,
                new VowelEncoder(new FibonacciGenerator()),
                CapsAlternator, new WordSplitter(new StaticDictionary()));

            return VerifyValue(_thorAlgorithm, guid);
        }

        dynamic PostCap(dynamic p)
        {
            Guid guid = GetGuid((string) p.guid, Allow.Duplicates);
            return PostValue(GetCaptainAmericaAlgorithm(guid), guid);
        }

        dynamic PostIronMan(dynamic p)
        {
            Guid guid = GetGuid((string) p.guid, Allow.Duplicates);
            return PostValue(_ironManAlgorithm, guid);
        }

        dynamic PostHulk(dynamic p)
        {
            Guid guid = GetGuid((string) p.guid, Allow.Duplicates);
            return PostValue(_theIncredibleHulkAlgorithm, guid);
        }

        dynamic PostThor(dynamic p)
        {
            Guid guid = GetGuid((string) p.guid, Allow.Duplicates);
            GetValueRequests previousRequest = GetMatchingPreviousRequest(guid);            
            _thorAlgorithm = new ThorAlgorithm(previousRequest.StartingFibonacciNumber,
                new VowelEncoder(new FibonacciGenerator()),
                CapsAlternator, new WordSplitter(new StaticDictionary()));

            return
                PostValue(
                    _thorAlgorithm, guid);
        }

        dynamic VerifyValue(IEncodingAlgorithm algorithm, Guid guid)
        {
            GetValueRequests previousRequest = GetMatchingPreviousRequest(guid);
            RemoveFromPreviousWordRequestList(previousRequest);
            
            if (!algorithm.GetType().Name.Contains(previousRequest.Algorithm.ToString()))
                throw new CandidateRequestException("You're using the wrong algorithm for this list of words");

            string ourEncoded = algorithm.Encode(previousRequest.Words.ToArray());
            return new {encoded = _base64Encoder.Encode(ourEncoded)};
        }

        dynamic GetValues(dynamic p)
        {
            Guid guid = GetGuid((string) p.guid);
            var rnd = new Random();
            var words = new List<string>();
            for (int i = 0; i < MaxWords; i++)
            {
                words.Add(CapsAlternator.Alternate(AllWords).ToArray()[rnd.Next(0, AllWords.Length - 1)]);
            }
            double startingFibonacciNumber = GetRandomFibonacciStartingNumber();
            AlgorithmName randomAlgorithmName = GetRandomAlgorithmName();
            WordRequests.Add(new GetValueRequests(guid, words, Convert.ToInt64(startingFibonacciNumber), randomAlgorithmName));
            return Response.AsJson(new {words, startingFibonacciNumber, algorithm = randomAlgorithmName.ToString()});
        }

        AlgorithmName GetRandomAlgorithmName()
        {
            var rnd = new Random();
            const int skipUnknownName = 1;
            return (AlgorithmName) rnd.Next(skipUnknownName, Enum.GetNames(typeof (AlgorithmName)).Length);
        }

        dynamic PostValue(IEncodingAlgorithm algorithm, Guid guid)
        {
            try
            {
                var reqBody = GetValidatedRequest();
                string candidateEncoded = reqBody.EncodedValue;
                string emailAddress = reqBody.EmailAddress;
                string webhookUrl = reqBody.WebhookUrl;
                string name = reqBody.Name;
                string repoUrl = reqBody.RepoUrl;
                
                GetValueRequests previousRequest = GetMatchingPreviousRequest(guid);

                if (!algorithm.GetType().Name.Contains(previousRequest.Algorithm.ToString()))
                    throw new CandidateRequestException("You're using the wrong algorithm for this list of words");

                RemoveFromPreviousWordRequestList(previousRequest);
                VerifyMatchingEncodedString(algorithm, previousRequest, candidateEncoded);
                AddSuccessToList(emailAddress, webhookUrl);

                int recentSuccesses = RecentSuccesses(emailAddress);
                bool isWinner = recentSuccesses >= RequiredSuccessCountForWin;
                bool repeatWinner =
                    Winners.Any(x => x.EmailAddress == emailAddress && x.Time > DateTime.UtcNow.AddMinutes(-60));

                if (isWinner && !repeatWinner)
                {
                    string randomFibonacciStartingNumber = GetRandomFibonacciStartingNumber().ToString();
                    SendWebhook(webhookUrl, randomFibonacciStartingNumber);
                    var winner = new Winner(emailAddress, name, repoUrl, DateTime.UtcNow);
                    NotifyHiringTeam(winner, randomFibonacciStartingNumber);
                    Winners.Add(winner);
                    Thread.Sleep(4000);
                }
                string message = repeatWinner
                    ? "Please wait 60 minutes before winning again to get the webhook post."
                    : isWinner ? WinMessage() : SingleSuccessMessage(recentSuccesses);
                string status = (isWinner ? PostAttempt.Winner : PostAttempt.Success).ToString();

                return Response.AsJson(new {status, message});
            }
            catch (WebhookException ex)
            {
                return CrashAndBurn(ex);
            }
            catch (CandidateRequestException ex)
            {
                return CrashAndBurn(ex);
            }
        }

        static void RemoveFromPreviousWordRequestList(GetValueRequests req)
        {
            WordRequests.Remove(req);
        }

        NewValueRequest GetValidatedRequest()
        {
            var reqBody = this.Bind<NewValueRequest>();

            if (string.IsNullOrEmpty(reqBody.EncodedValue)) throw new ArgumentNullException("encodedValue");
            if (string.IsNullOrEmpty(reqBody.EmailAddress)) throw new ArgumentNullException("emailAddress");
            if (string.IsNullOrEmpty(reqBody.WebhookUrl)) throw new ArgumentNullException("webhookUrl");
            if (string.IsNullOrEmpty(reqBody.Name)) throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(reqBody.RepoUrl)) throw new ArgumentNullException("repoUrl");
            return reqBody;
        }

        Response CrashAndBurn(Exception ex)
        {
            return Response.AsJson(new {status = "CrashAndBurn", message = ex.Message}, HttpStatusCode.BadRequest);
        }

        void NotifyHiringTeam(Winner winner, string secretCode)
        {
            _notifier.Notify(winner.EmailAddress, winner.Name, winner.RepoUrl, secretCode);
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

        void VerifyMatchingEncodedString(IEncodingAlgorithm algorithm, GetValueRequests previousRequest,
            string candidateEncoded)
        {
            string ourEncoded = algorithm.Encode(previousRequest.Words.ToArray());
            string b64Encoded = _base64Encoder.Encode(ourEncoded);
            bool candidateFailedToProvideCorrectEncodedString = candidateEncoded != b64Encoded;
            if (candidateFailedToProvideCorrectEncodedString)
                throw new CandidateRequestException("The encoded string did not match.");
        }

        static GetValueRequests GetMatchingPreviousRequest(Guid guid)
        {
            GetValueRequests previousRequest = WordRequests.FirstOrDefault(x => x.Guid == guid);
            if (previousRequest == null)
                throw new CandidateRequestException("There were no previous requests for that guid.");
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

        void SendWebhook(string webhookUrl, string randomFibonacciStartingNumber)
        {
            var client = new RestClient(webhookUrl);
            var restRequest = new RestRequest {RequestFormat = DataFormat.Json};
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddBody(new
                                {
                                    secret = string.Format("{0} is the magic!", randomFibonacciStartingNumber)
                                });
            IRestResponse restResponse = client.Post(restRequest);
            if ((int) restResponse.StatusCode != 200)
            {
                throw new WebhookException();
            }
        }

        static Guid GetGuid(string guidString, Allow allow = Allow.OnlyUnique)
        {
            Guid guid;
            if (!Guid.TryParse(guidString, out guid))
                throw new CandidateRequestException("The guid provided was not valid.");
            if (allow == Allow.OnlyUnique && WordRequests.Any(x => x.Guid.ToString() == guid.ToString()))
                throw new CandidateRequestException("The guid needed to be unique, but has been used before.");
            return guid;
        }
    }
}