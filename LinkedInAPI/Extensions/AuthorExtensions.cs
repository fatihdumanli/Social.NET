using SocialMediaSharing.BLL.LinkedInAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI.Extensions
{
    public static class AuthorExtensions
    {
        public static string ToOwnerInfo(this string accountId, LinkedInAccountType personalOrOrganization)
        {
            if (personalOrOrganization.Equals(LinkedInAccountType.Personal))
            {
                return $"urn:li:person:{accountId}";
            }

            else if (personalOrOrganization.Equals(LinkedInAccountType.Organization))
            {
                return $"urn:li:organization:{accountId}";
            }

            else
                throw new InvalidOperationException("Something interesting happened in AuthorExtensions...");
        }
    }
}
