using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using SocialMediaSharing.BLL.OAuth;
using System;
using System.Web;

namespace SocialMediaSharing.BLL
{
    public class OAuthv1Client : IOAuthv1Client
    {
        public OAuthRequestResult<string> AccessResource(string endpoint, string consumerKey, string consumerSecret)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// To get a user specific access token
        /// https://api.twitter.com/oauth/access_token
        /// </summary>
        /// <param name="requestTokenEndpoint"></param>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="oauthToken"></param>
        /// <param name="oauthVerifier"></param>
        /// <returns></returns>
        public OAuthRequestResult<OAuthv1AccessToken> RequestAccessToken(string requestTokenEndpoint, string consumerKey, string consumerSecret, string oauthToken, string oauthVerifier)
        {
            var _oauthRestClient = new RestClient(requestTokenEndpoint)
            {
                Authenticator = OAuth1Authenticator
                .ForAccessToken(consumerKey, consumerSecret, oauthToken, "", oauthVerifier)
            };

            var request = new RestRequest(Method.POST);
            try
            {
                var response = _oauthRestClient.Execute(request);

                if (!response.IsSuccessful)
                {
                    return OAuthRequestResult<OAuthv1AccessToken>.Fail(response.Content);
                }

                var parsed = HttpUtility.ParseQueryString(response.Content);
                var tokenStr = parsed["oauth_token"];
                var secretStr = parsed["oauth_token_secret"];
                var userId = parsed["user_id"];
                var screenName = parsed["screen_name"];

                var token = OAuthv1AccessToken.Create(tokenStr, secretStr, userId, screenName);
                return OAuthRequestResult<OAuthv1AccessToken>.Success(token);

            }
            catch (Exception ex)
            {
                return OAuthRequestResult<OAuthv1AccessToken>.Fail(ex.Message);
            }

        }

        /// <summary>
        /// https://api.twitter.com/oauth/request_token
        /// </summary>
        /// <param name="requestTokenEndpoint"></param>
        /// <param name="consumerKey"></param>
        /// <param name="consumerSecret"></param>
        /// <param name="callbackUrl"></param>
        public OAuthRequestResult<OAuthv1Token> RequestToken(string requestTokenEndpoint, string consumerKey, string consumerSecret, string callbackUrl)
        {
            var _oauthRestClient = new RestClient(requestTokenEndpoint)
            {
                Authenticator = OAuth1Authenticator
                .ForRequestToken(consumerKey,
                consumerSecret,
                callbackUrl)
            };

            var request = new RestRequest(Method.POST);

            try
            {
                var response = _oauthRestClient.Execute(request);

                if (!response.IsSuccessful)
                {
                    return OAuthRequestResult<OAuthv1Token>.Fail(response.Content);
                }

                var parsed = HttpUtility.ParseQueryString(response.Content);
                var tokenStr = parsed["oauth_token"];
                var secretStr = parsed["oauth_token_secret"];

                var token = OAuthv1Token.Create(tokenStr, secretStr);

                return OAuthRequestResult<OAuthv1Token>.Success(token);
            }
            catch (Exception ex)
            {

                return OAuthRequestResult<OAuthv1Token>.Fail(ex.Message);
            }

        }
    }
}
