using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.FacebookGraph.Entities
{
   // https://developers.facebook.com/docs/graph-api/reference/video
   // A photo must be less than 10MB in size.
   public class FacebookVideo
   {
      [JsonProperty("id")]
      public string Id { get; set; }
      [JsonProperty("permalink_url")]
      public string VideoURL { get; set; }
      [JsonProperty("created_time")]
      public DateTime CreatedTime { get; set; }

      [JsonProperty("published")]
      public bool IsPublished { get; set; } = true;
   }
}
