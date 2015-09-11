using System.Collections.Generic;

namespace SlackClient
{
    public class SlackPost
    {
        public SlackPost()
        {
            attachments = new List<SlackPostAttachment>();
        }

        public string channel { get; set; }

        public string username { get; set; }

        public string text { get; set; }

        public string icon_emoji { get; set; }

        public List<SlackPostAttachment> attachments { get; set; }
        
        public string icon_url { get; set; }
    }
}