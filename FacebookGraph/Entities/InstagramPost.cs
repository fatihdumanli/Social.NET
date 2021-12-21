using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.FacebookGraph.Entities
{
    public class InstagramPost
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("permalink")]
        public string Permalink { get; set; }
    }
}
