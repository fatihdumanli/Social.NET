using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI.Entities
{
    public class LinkedInPost : ILinkedInAPIEntity
    {
        public LinkedInPost()
        {
        }

        [JsonProperty("content")]
        public LinkedInContent Content { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("subject")]
        public string Subject { get; set; }

        [JsonProperty("text")]
        public LinkedInText Text { get; set; }

    }
}
