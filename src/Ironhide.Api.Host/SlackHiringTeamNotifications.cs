using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SlackClient;

namespace Ironhide.Api.Host
{
    public class SlackHiringTeamNotifications : IHiringTeamNotifier
    {
        const string Icon = "http://pre14.deviantart.net/aac4/th/pre/i/2010/111/b/3/napoleon_dynamite_by_kcbonx.jpg";
        readonly ISlackLogger _slackClient;

        public SlackHiringTeamNotifications()
        {
            _slackClient = new SlackLogger();
        }

        public void Notify(string emailAddress, string name, string repoUrl, string code, TimeSpan timeToWinFromFirstSuccess)
        {
            const string message =
                "Hi future teammates! I just finished the dev challenge. BTW, what are you guys smoking over there?? I should be sending the hiring team an email soon with a link to my site with the secret code (that matches below).";

            var attachment = new SlackPostAttachment()
                             {
                                 color = "green",
                                 fallback = message,
                                 //text = message,
                                 fields = new List<AttachmentField>{
                                              new AttachmentField()
                                              {
                                                  title = "Secret Code",
                                                  value = code
                                              },
                                              new AttachmentField
                                              {
                                                  title = "Email Address",
                                                  value = emailAddress
                                              },
                                              new AttachmentField
                                              {
                                                  title = "My Code",
                                                  value = repoUrl
                                              },
                                              new AttachmentField
                                              {
                                                  title = "Milliseconds to Win",
                                                  value = timeToWinFromFirstSuccess.TotalMilliseconds.ToString()
                                              }
                                          }
                             };

            var sendMessage = _slackClient.Log(new SlackPost()
                                                     {
                                                         attachments = new List<SlackPostAttachment> {attachment},
                                                         channel = "#hiring",
                                                         text = message,
                                                         username = "Dev Candidate: " + name,
                                                         icon_url = Icon,
                                                     });

        }
    }
}