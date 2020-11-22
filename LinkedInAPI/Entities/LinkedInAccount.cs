using SocialMediaSharing.BLL.LinkedInAPI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI.Entities
{
    public enum LinkedInAccountType
    {
        Personal, Organization
    }

    public class LinkedInAccount
    {
        public static LinkedInAccount Create(string userId, LinkedInAccountType personalOrOrganization)
        {
            return new LinkedInAccount() { UserId = userId, PersonalOrOrganization = personalOrOrganization };
        }

        private LinkedInAccount() { }
        
        public string UserId { get; set; }
        public LinkedInAccountType PersonalOrOrganization { get; set; }
    }
}
