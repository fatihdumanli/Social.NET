using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.TwitterAPI.Entities
{

    public class TwitterImage
    {
        [JsonProperty("image_type")]
        public string ImageType { get; set; }
        [JsonProperty("w")]
        public decimal W { get; set; }
        [JsonProperty("h")]
        public decimal H { get; set; }
    }
    public class TwitterMedia
    {
        [JsonProperty("media_id")]
        public string MediaId { get; set; }
        [JsonProperty("size")]
        public string Size { get; set; }
        [JsonProperty("expires_after_secs")]
        public string ExpiresAfterSecs { get; set; }
        [JsonProperty("image")]
        public TwitterImage Image { get; set; }

    }
}
