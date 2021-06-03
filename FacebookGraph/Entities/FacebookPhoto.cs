using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.FacebookGraph.Entities
{
   // https://developers.facebook.com/docs/graph-api/reference/photo/
   // A photo must be less than 10MB in size.
   public class FacebookPhoto
   {
      [JsonProperty("id")]
      public string Id { get; set; }
      [JsonProperty("post_id")]
      public string PostID { get; set; } = null;
      [JsonProperty("link")]
      public string PhotoURL { get; set; }
      [JsonProperty("alt_text")]
      public string AltText { get; set; }
      [JsonProperty("created_time")]
      public DateTime CreatedTime { get; set; }

      [JsonProperty("published")]
      public bool IsPublished { get; set; } = true;
   }
}
