using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.FacebookGraph.Entities
{
    public class FacebookPost
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("created_time")]
        public DateTime CreatedTime { get; set; }

        [JsonProperty("permalink_url")]
        public string PermalinkURL {get;set;}
    }
}
