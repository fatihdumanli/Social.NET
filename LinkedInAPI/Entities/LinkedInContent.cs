using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI.Entities
{
    public class LinkedInContent
    {
        public LinkedInContent()
        {
            ContentEntities = new List<LinkedInContentEntity>();
            ContentEntities.Add(new LinkedInContentEntity());
        }

        [JsonProperty("contentEntities")]
        public List<LinkedInContentEntity> ContentEntities { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }

      

    }
}
