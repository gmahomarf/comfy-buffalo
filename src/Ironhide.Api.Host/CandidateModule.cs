using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;

namespace Ironhide.Api.Host
{
    public class CandidateModule : NancyModule
    {
        const string FailureMessage =
            "Hello candidate! Thanks for playing. However, something was not right. Check everything and try again.";

        static List<GetValueRequests> _getValueRequests = new List<GetValueRequests>();

        static string[] _allWords = new string[]
                                    {
                                        "ashuph", "goohee", "nygnas", "sherga", "cowhyw", "eerdox", "lekops", "gledsy",
                                        "iwefty", "yjyltu", "olapev", "ooptyn", "oansol", "xezyms", "eeping", "suckyj",
                                        "divorce","frequent","drown","sharp","blushing","communication","decoder","internal","column","agreeable",
                                        "pharaoh","femur","bird","frightening","bat","gum","gobbling","diplomatic","downriver",
                                        "healthy","liquid","flush","after","insurance","beam","harm","shotgun","business","coal" 
                                    };

        const int MaxWords = 10;

        public CandidateModule()
        {
            Get["values/{guid}"] =
                p =>
                {
                    var guidString = (string)p.guid;
                    Guid guid;
                    var isValidGuid = Guid.TryParse(guidString, out guid);
                    if (!isValidGuid) return FailureResponse();
                    if (_getValueRequests.Any(x => x.Guid == guid)) return FailureResponse();
                    _getValueRequests.Add(new GetValueRequests(guid));

                    var rnd = new Random();
                    var words = new List<string>();
                    for (int i = 0; i < MaxWords; i++)
                    {
                        words.Add(_allWords[rnd.Next(0, _allWords.Length - 1)]);
                    }
                    return Response.AsJson(new { words });
                };
        }

        Response FailureResponse()
        {
            return Response.AsJson(new { message = FailureMessage }, HttpStatusCode.BadRequest);
        }
    }
}