using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.FacebookGraph.Entities
{
    public class FacebookUploadVideoResponse
    {
        [JsonProperty("id")]
        public string VideoId { get; set; }
    }
}
