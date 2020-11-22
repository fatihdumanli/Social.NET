using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.OAuthv2
{
    public interface IOAuthv2Client
    {
        OAuthRequestResult<OAuthv2AccessToken> RequestAccessToken(string requestTokenEndpoint,
            string clientId,
            string clientSecret,
            string authorizationCode,
            string redirectUri);
    }
}
