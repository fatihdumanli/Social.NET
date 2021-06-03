using Newtonsoft.Json;
using SocialMediaSharing.BLL.LinkedInAPI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI.Entities.UGCPost
{
       public class CreateUGCPostResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }    
}
