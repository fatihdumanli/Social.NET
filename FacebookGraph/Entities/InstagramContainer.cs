using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.FacebookGraph.Entities
{
    // Represents a media container.
    // https://developers.facebook.com/docs/instagram-api/reference/ig-container
    public class InstagramContainer
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("status_code")]
        public string StatusCode { get; set; }
    }
    public enum InstagramContainerType{
        VIDEO,
        PHOTO
    }
}
