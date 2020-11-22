using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.OAuthv2
{
    public class OAuthv2Client : IOAuthv2Client
    {

        /// <summary>
        /// Used to request an OAuthv1.0 access token
        /// </summary>
        /// <param name="requestTokenEndpoint"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="authorizationCode"></param>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        public OAuthRequestResult<OAuthv2AccessToken> RequestAccessToken(string requestTokenEndpoint, 
            string clientId, 
            string clientSecret, 
            string authorizationCode,
            string redirectUri)
        {
            var _ouathv2Client = new RestClient(requestTokenEndpoint)
            {
            };

            var request = new RestRequest(Method.POST);
            request.AddParameter("grant_type", "authorization_code");
            request.AddParameter("code", authorizationCode);
            request.AddParameter("redirect_uri", redirectUri);
            request.AddParameter("client_id", clientId);
            request.AddParameter("client_secret", clientSecret);

            try
            {
                var response = _ouathv2Client.Execute(request);

                if (!response.IsSuccessful)
                {
                    return OAuthRequestResult<OAuthv2AccessToken>.Fail(response.Content);
                }

                var token = JsonConvert.DeserializeObject<OAuthv2AccessToken>(response.Content);
                token.ExpiresOn = DateTime.UtcNow.AddSeconds(token.ExpiresIn);

                return OAuthRequestResult<OAuthv2AccessToken>.Success(token);
            }
            catch (Exception ex)
            {
                return OAuthRequestResult<OAuthv2AccessToken>.Fail(ex.Message);
            }
       

        }
    }
}
