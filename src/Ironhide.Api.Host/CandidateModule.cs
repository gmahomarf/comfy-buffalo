using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy;
using Nancy.ModelBinding;
using RestSharp;

namespace Ironhide.Api.Host
{
    public class CandidateModule : NancyModule
    {
        const int MaxWords = 10;
        const decimal RequiredSuccessCountForWin = 20;
        const double MillisecondsForWin = 2000;

        static readonly List<GetValueRequests> GetValueRequests = new List<GetValueRequests>();

        static readonly string[] AllWords =
        {
            "ashuph", "goohee", "nygnas", "sherga", "cowhyw", "eerdox", "lekops", "gledsy",
            "iwefty", "yjyltu", "olapev", "ooptyn", "oansol", "xezyms", "eeping", "suckyj",
            "divorce", "frequent", "drown", "sharp", "blushing", "communication", "decoder", "internal", "column",
            "agreeable",
            "pharaoh", "femur", "bird", "frightening", "bat", "gum", "gobbling", "diplomatic", "downriver",
            "healthy", "liquid", "flush", "after", "insurance", "beam", "harm", "shotgun", "business", "coal"
        };

        readonly List<CandidateSuccess> _candidateSuccesses = new List<CandidateSuccess>();
        readonly SuperSecretEncodingAlgorithm _encoder;
        readonly FibonacciGenerator _fibonacciGenerator;
        readonly Base64StringEncoder _base64Encoder;

        public CandidateModule()
        {
            _encoder = new SuperSecretEncodingAlgorithm(new VowelEncoder(new FibonacciGenerator()), new VowelShifter(), new CapsAlternator(), new AsciiValueDelimiterAdder());
            _fibonacciGenerator = new FibonacciGenerator();
            _base64Encoder = new Base64StringEncoder();

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
                words.Add(AllWords[rnd.Next(0, AllWords.Length - 1)]);
            }
            var startingFibonacciNumber = GetRandomFibonacciStartingNumber();
            GetValueRequests.Add(new GetValueRequests(guid, words, startingFibonacciNumber));
            return Response.AsJson(new {words, startingFibonacciNumber});
        }

        async Task<dynamic> PostValue(dynamic p)
        {
            Guid guid = GetGuid((string) p.guid, Allow.Duplicates);
            var reqBody = this.Bind<NewValueRequest>();
            string candidateEncoded = reqBody.EncodedValue;
            string emailAddress = reqBody.EmailAddress;
            string webhookUrl = reqBody.WebhookUrl;

            GetValueRequests previousRequest = GetMatchingPreviousRequest(guid);
            VerifyMatchingEncodedString(previousRequest, candidateEncoded);
            AddSuccessToList(emailAddress, webhookUrl);

            int recentSuccesses = RecentSuccesses(emailAddress);
            bool isWinner = recentSuccesses >= RequiredSuccessCountForWin;
            if (isWinner) await SendWebhook(webhookUrl);
            string message = isWinner ? WinMessage() : SingleSuccessMessage(recentSuccesses);
            PostAttempt status = isWinner ? PostAttempt.Winner : PostAttempt.Success;
            return Response.AsJson(new {status, message});
        }


        double GetRandomFibonacciStartingNumber()
        {
            var numbers = _fibonacciGenerator.Generate(40).ToArray();
            var rnd = new Random();
            return numbers[rnd.Next(0, numbers.Count())];
        }

        void AddSuccessToList(string emailAddress, string webhookUrl)
        {
            _candidateSuccesses.Add(new CandidateSuccess(emailAddress, webhookUrl, DateTime.UtcNow));
        }

        void VerifyMatchingEncodedString(GetValueRequests previousRequest, string candidateEncoded)
        {
            string ourEncoded = _encoder.Encode(previousRequest.StartingFibonacciNumber, previousRequest.Words.ToArray());
            var b64Encoded = _base64Encoder.Encode(ourEncoded);
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
                _candidateSuccesses.Where(x => x.EmailAddress == emailAddress && x.Time >= minimumWinTime);

            return recentSuccesses.Count();
        }

        async Task SendWebhook(string webhookUrl)
        {
            var client = new RestClient(webhookUrl);
            var restRequest = new RestRequest();
            double randomFibonacciStartingNumber = GetRandomFibonacciStartingNumber();
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