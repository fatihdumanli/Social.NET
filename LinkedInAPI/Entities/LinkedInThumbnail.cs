using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI.Entities
{
    public class LinkedInThumbnail
    {
        [JsonProperty("resolvedUrl")]
        public string ResolvedUrl { get; set; }

    }
}
