using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.OAuthv2
{
    public class OAuthv2AccessToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
        [JsonProperty("expires_in")]
        public double ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresOn { get; set; }
        public string Scope { get; set; }

        [JsonIgnore]
        public bool IsExpired
        {
            get
            {
                return DateTime.UtcNow >= ExpiresOn;
            }
        }


        public static OAuthv2AccessToken Create(string accessToken)
        {
            return new OAuthv2AccessToken() { AccessToken = accessToken };
        }

        public static OAuthv2AccessToken Create(string accessToken, double expiresIn, string refreshToken)
        {
            return new OAuthv2AccessToken() { AccessToken = accessToken, ExpiresIn = expiresIn, RefreshToken = refreshToken, ExpiresOn = DateTime.UtcNow.AddSeconds(expiresIn) };
        }

        public static OAuthv2AccessToken Create(string accessToken, DateTime expiresOn, string refreshToken)
        {
            return new OAuthv2AccessToken() { AccessToken = accessToken, ExpiresOn = expiresOn, RefreshToken = refreshToken };
        }

    }
}
