using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI.Entities
{
    public class LinkedInProfilePicture
    {
        [JsonProperty("displayImage")]
        public string DisplayImage { get; set; }
    }

    public class LinkedInPublicInformation : ILinkedInAPIEntity
    {
        [JsonProperty("localizedFirstName")]
        public string FirstName { get; set; }
        [JsonProperty("localizedLastName")]
        public string LastName { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("profilePicture")]
        public LinkedInProfilePicture ProfilePicture { get; set; }
    }
}
