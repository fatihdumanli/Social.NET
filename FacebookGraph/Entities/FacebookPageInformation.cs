using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.FacebookGraph.Entities
{
    public class FacebookPageInformation
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("tasks")]
        List<string> Tasks { get; set; }

        public static FacebookPageInformation Create(string accessToken, string name, string id)
        {
            return new FacebookPageInformation() { AccessToken = accessToken, Name = name, Id = id };
        }

    }
}
