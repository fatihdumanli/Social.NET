
using Newtonsoft.Json;

namespace SocialMediaSharing.BLL.OAuth
{
    /// <summary>
    /// Used only when requesting a access token.
    /// </summary>
    public class OAuthv1Token
    {
        [JsonProperty("oauth_token")]
        public string Token { get; set; }

        [JsonProperty("oauth_token_secret")]
        public string Secret { get; set; }

        private OAuthv1Token() { }

        public static OAuthv1Token Create(string token, string secret)
        {
            return new OAuthv1Token() { Token = token, Secret = secret };
        }
    }
}
