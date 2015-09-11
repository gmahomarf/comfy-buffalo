using System.Collections.Generic;

namespace SlackClient
{
    public class SlackPostAttachment
    {
        public SlackPostAttachment()
        {
            fields = new List<AttachmentField>();
        }

        public List<AttachmentField> fields { get; set; }

        public string color { get; set; }

        public string pretext { get; set; }

        public string text { get; set; }

        public string fallback { get; set; }
    }
}