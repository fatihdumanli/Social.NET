using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.OAuth
{
    public class OAuthv1AccessToken
    {
        public string OAuthToken { get; private set; }
        public string OAuthSecret { get; private set; }
        public string UserId { get; set; }
        public string ScreenName { get; set; }

        private OAuthv1AccessToken() { }
        public static OAuthv1AccessToken Create(string token, string secret, string userId, string screenName)
        {
            return new OAuthv1AccessToken() { OAuthToken = token, OAuthSecret = secret, UserId = userId, ScreenName = screenName };
        }

        public static OAuthv1AccessToken Create(string token, string secret)
        {
            return new OAuthv1AccessToken() { OAuthToken = token, OAuthSecret = secret };
        }

    }
}
