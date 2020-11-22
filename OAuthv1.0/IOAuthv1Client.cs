using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.OAuth
{
    public interface IOAuthv1Client
    {
        OAuthRequestResult<OAuthv1AccessToken> RequestAccessToken(string requestTokenEndpoint, string consumerKey, string consumerSecret, string oauthToken, string oauthVerifier);

        OAuthRequestResult<OAuthv1Token> RequestToken(string requestTokenEndpoint, string consumerKey, string consumerSecret, string callbackUrl);

        OAuthRequestResult<string> AccessResource(string endpoint, string consumerKey, string consumerSecret);
    }
}
