using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI.Entities
{
    public class LinkedInContentEntity
    {
        public LinkedInContentEntity()
        {
            Thumbnails = new List<LinkedInThumbnail>();
            Thumbnails.Add(new LinkedInThumbnail());
        }

        [JsonProperty("entityLocation")]
        public string EntityLocation { get; set; }
        [JsonProperty("thumbnails")]
        public List<LinkedInThumbnail> Thumbnails { get; set; }


    }
}
