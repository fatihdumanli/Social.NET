using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using SocialMediaSharing.BLL.FacebookGraph.Entities;
using SocialMediaSharing.BLL.OAuthv2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.FacebookGraph
{
    public class FacebookGraphClient : IFacebookGraphClient
    {
        #region Identification
        /// <summary>
        /// https://developers.facebook.com/docs/facebook-login/access-tokens/refreshing/
        /// </summary>
        /// <param name="token"></param>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public FacebookGraphResult<FacebookLongLivedToken> GetLongLivedToken(OAuthv2AccessToken token, string appId, string appSecret)
        {
            var _fbRestClient = new RestClient("https://graph.facebook.com/v8.0/oauth/access_token")
            {
            };

            var request = new RestRequest(Method.GET);
            request.AddParameter("grant_type", "fb_exchange_token");
            request.AddParameter("client_id", appId);
            request.AddParameter("client_secret", appSecret);
            request.AddParameter("fb_exchange_token", token.AccessToken);


            try
            {
                var response = _fbRestClient.Execute(request);

                if (!response.IsSuccessful)
                {
                    return FacebookGraphResult<FacebookLongLivedToken>.Fail(response.Content);
                }
              
                var longLivedToken = JsonConvert.DeserializeObject<FacebookLongLivedToken>(response.Content);

                return FacebookGraphResult<FacebookLongLivedToken>.Success(longLivedToken);
            }
            catch (Exception e)
            {
                return FacebookGraphResult<FacebookLongLivedToken>.Fail(e.Message);
            }
        }

        /// <summary>
        /// https://developers.facebook.com/docs/pages/managing
        /// Works with user access_token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public FacebookGraphResult<List<FacebookPageInformation>> GetPageList(OAuthv2AccessToken token)
        {
            var _fbRestClient = new RestClient("https://graph.facebook.com/me/accounts")
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };

            var request = new RestRequest(Method.GET);
            try
            {
                var response = _fbRestClient.Execute(request);

                if (!response.IsSuccessful)
                {
                    return FacebookGraphResult<List<FacebookPageInformation>>.Fail(response.Content);
                }

                JObject obj = JObject.Parse(response.Content);
                var data = obj["data"];
                var paging = obj["paging"];

                var pageList = JsonConvert.DeserializeObject<List<FacebookPageInformation>>(data.ToString());

                return FacebookGraphResult<List<FacebookPageInformation>>.Success(pageList);
            }
            catch (Exception e)
            {
                return FacebookGraphResult<List<FacebookPageInformation>>.Fail(e.Message);
            }
        }

        /// <summary>
        /// For fields: https://developers.facebook.com/docs/graph-api/reference/user/#fields
        /// </summary>
        /// <param name="token"></param>
        /// <param name="fields"></param>
        public FacebookGraphResult<FacebookPublicInformation> GetPublicInformation(OAuthv2AccessToken token, string[] fields)
        {
            var _fbRestClient = new RestClient(string.Format("https://graph.facebook.com/me?fields={0}", string.Join(",", fields)))
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };

            var request = new RestRequest(Method.POST);

            try
            {
                var response = _fbRestClient.Execute(request);

                if (!response.IsSuccessful)
                {
                    FacebookGraphResult<FacebookPublicInformation>.Fail(response.Content);
                }

                var entity = JsonConvert.DeserializeObject<FacebookPublicInformation>(response.Content);

                return FacebookGraphResult<Entities.FacebookPublicInformation>.Success(entity);
            }
            catch (Exception ex)
            {
                return FacebookGraphResult<Entities.FacebookPublicInformation>.Fail(ex.Message);
            }
           
        }

        #endregion


        /// <summary>
        /// To get scheduled posts.
        /// https://graph.facebook.com/101389545124742/scheduled_posts
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public FacebookGraphResult<List<FacebookPost>> GetScheduledPosts(OAuthv2AccessToken token, FacebookPageInformation account)
        {
            var _fbRestClient = new RestClient(string.Format("https://graph.facebook.com/{0}/scheduled_posts",
                              account.Id))
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };

            var request = new RestRequest(Method.GET);

            try
            {
                var response = _fbRestClient.Execute(request);
                if (!response.IsSuccessful)
                {
                    return FacebookGraphResult<List<FacebookPost>>.Fail(response.Content);
                }

                JObject result = JObject.Parse(response.Content);
                var posts = JsonConvert.DeserializeObject<List<FacebookPost>>(result["data"].ToString());

                return FacebookGraphResult<List<FacebookPost>>.Success(posts);

            }
            catch (Exception ex)
            {
                return FacebookGraphResult<List<FacebookPost>>.Fail(ex.Message);
            }
        }


        #region Publish Immediate Post
        /// <summary>
        /// For pages.
        /// https://developers.facebook.com/docs/pages/publishing/
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="content"></param>
        /// <returns></returns
        public FacebookGraphResult<bool> PublishPost(OAuthv2AccessToken token, FacebookPageInformation account, string content)
        {
            var _fbRestClient = new RestClient(string.Format("https://graph.facebook.com/{0}/feed?message={1}&access_token={2}",
                               account.Id, content, account.AccessToken))
            {
            };

            var request = new RestRequest(Method.POST);

            try
            {
                var response = _fbRestClient.Execute(request);
                if(!response.IsSuccessful)
                {
                    return FacebookGraphResult<bool>.Fail(response.Content);
                }

                return FacebookGraphResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return FacebookGraphResult<bool>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Text and image
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        /// <param name="content"></param>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public FacebookGraphResult<bool> PublishPost(OAuthv2AccessToken token, FacebookPageInformation account, string content, string imageUrl)
        {
            var _fbRestClient = new RestClient($"https://graph.facebook.com/{account.Id}/photos?url={imageUrl}&message={content}&access_token={token.AccessToken}")
            {
            };

            var request = new RestRequest(Method.POST);

            try
            {
                var response = _fbRestClient.Execute(request);
                if (!response.IsSuccessful)
                {
                    return FacebookGraphResult<bool>.Fail(response.Content);
                }

                return FacebookGraphResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return FacebookGraphResult<bool>.Fail(ex.Message);
            }

        }

        /// <summary>
        /// Link and comment (text)
        /// </summary>
        /// <param name="token"></param>
        /// <param name="content"></param>
        /// <param name="account"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public FacebookGraphResult<bool> PublishPost(OAuthv2AccessToken token, string content, FacebookPageInformation account, string link)
        {

            var _fbRestClient = new RestClient($"https://graph.facebook.com/{account.Id}/feed?link={link}&message={content}&access_token={token.AccessToken}")
            {
            };

            var request = new RestRequest(Method.POST);

            try
            {
                var response = _fbRestClient.Execute(request);
                if (!response.IsSuccessful)
                {
                    return FacebookGraphResult<bool>.Fail(response.Content);
                }

                return FacebookGraphResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return FacebookGraphResult<bool>.Fail(ex.Message);
            }

        }

        /// <summary>
        /// Publishes a post with a video.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="videoPath"></param>
        /// <param name="description"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        public FacebookGraphResult<bool> PublishPost(OAuthv2AccessToken token, string videoPath, string description, FacebookPageInformation account)
        {

            var uploadResult = UploadVideo(token, account, videoPath, description);

            if (!uploadResult.IsSuccessful)
                return FacebookGraphResult<bool>.Fail("An error has occured while trying to upload video to Facebook. Details: " + uploadResult.Message);

            var _fbRestClient = new RestClient($"https://graph.facebook.com/{account.Id}/videos")
            {
            };

            var request = new RestRequest(Method.POST);
            request.AddParameter("id", uploadResult.Entity.VideoId);

            try
            {
                //BadRequest dönmesine rağmen başarılı bu. (buraya exceptional bir şey eklemeliyiz.)
                //Facebook dev ekibiyle iletişime de geçilebilir.
                var response = _fbRestClient.Execute(request);

                //if(response.StatusCode.Equals(HttpStatusCode.BadRequest))
                //if (!response.IsSuccessful)
                //{
                //    return FacebookGraphResult<bool>.Fail(response.Content);
                //}

                return FacebookGraphResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return FacebookGraphResult<bool>.Fail(ex.Message);
            }
        }
        #endregion


        #region Publish Scheduled Posts
        /// <summary>
        /// ONLY TEXT CONTENT
        /// Schedules a post.
        /// https://developers.facebook.com/docs/pages/publishing/
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public FacebookGraphResult<bool> SchedulePost(OAuthv2AccessToken token, FacebookPageInformation account, string content, long unixTimeStamp)
        {
            var _fbRestClient = new RestClient(string.Format("https://graph.facebook.com/{0}/feed?published=false&message={1}&scheduled_publish_time={2}",
                               account.Id, content, unixTimeStamp))
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };

            var request = new RestRequest(Method.POST);

            try
            {
                var response = _fbRestClient.Execute(request);
                if (!response.IsSuccessful)
                {
                    return FacebookGraphResult<bool>.Fail(response.Content);
                }

                return FacebookGraphResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return FacebookGraphResult<bool>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// IMAGE - TEXT
        /// Schedules a post.
        /// https://developers.facebook.com/docs/pages/publishing/
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public FacebookGraphResult<bool> SchedulePost(OAuthv2AccessToken token, FacebookPageInformation account, string content, string imageUrl, long unixTimeStamp)
        {
            var _fbRestClient = new RestClient($"https://graph.facebook.com/{account.Id}/photos?url={imageUrl}&published=false&message={content}&scheduled_publish_time={unixTimeStamp}")
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };

            var request = new RestRequest(Method.POST);

            try
            {
                var response = _fbRestClient.Execute(request);
                if (!response.IsSuccessful)
                {
                    return FacebookGraphResult<bool>.Fail(response.Content);
                }

                return FacebookGraphResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return FacebookGraphResult<bool>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// LINK - TEXT
        /// Schedules a post.
        /// https://developers.facebook.com/docs/pages/publishing/
        /// </summary>
        /// <param name="token"></param>
        /// <param name="link"></param>
        /// <param name="content"></param>
        /// <param name="account"></param>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public FacebookGraphResult<bool> SchedulePost(OAuthv2AccessToken token, string link, string content, FacebookPageInformation account, long unixTimeStamp)
        {
            var _fbRestClient = new RestClient($"https://graph.facebook.com/{account.Id}/feed?link={link}&published=false&message={content}&scheduled_publish_time={unixTimeStamp}")
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };

            var request = new RestRequest(Method.POST);

            try
            {
                var response = _fbRestClient.Execute(request);
                if (!response.IsSuccessful)
                {
                    return FacebookGraphResult<bool>.Fail(response.Content);
                }

                return FacebookGraphResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return FacebookGraphResult<bool>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// VIDEO-DESCRIPTION. 
        /// Schedules a post.
        /// https://developers.facebook.com/docs/pages/publishing/
        /// </summary>
        /// <param name="token"></param>
        /// <param name="link"></param>
        /// <param name="content"></param>
        /// <param name="account"></param>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public FacebookGraphResult<bool> SchedulePost(OAuthv2AccessToken token, string pathToVideo, FacebookPageInformation account, string description, long unixTimeStamp)
        {
            var uploadVideoResult = UploadVideo(token, account, pathToVideo, description, unixTimeStamp);

            if (!uploadVideoResult.IsSuccessful)
                return FacebookGraphResult<bool>.Fail("An error has occured while trying to upload video to facebook via Graph API. Details: " + uploadVideoResult.Message);


            var _fbRestClient = new RestClient($"https://graph.facebook.com/{account.Id}/videos")
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };

            var request = new RestRequest(Method.POST);
            request.AddParameter("id", uploadVideoResult.Entity.VideoId);

            try
            {
                //BadRequest dönmesine rağmen başarılı bu. (buraya exceptional bir şey eklemeliyiz.)
                //Facebook dev ekibiyle iletişime de geçilebilir.
                var response = _fbRestClient.Execute(request);

                //if(response.StatusCode.Equals(HttpStatusCode.BadRequest))
                //if (!response.IsSuccessful)
                //{
                //    return FacebookGraphResult<bool>.Fail(response.Content);
                //}

                return FacebookGraphResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return FacebookGraphResult<bool>.Fail(ex.Message);
            }
        }

        #endregion

        #region Video
        /// <summary>
        /// FOR CHUNKED UPLOAD.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="page"></param>
        /// <param name="fileSizeInBytes"></param>
        /// <returns></returns>
        public FacebookGraphResult<FacebookUploadSession> StartVideoUploadSession(OAuthv2AccessToken token, FacebookPageInformation page, string fileSizeInBytes)
        {
            var _fbRestClient = new RestClient($"https://graph-video.facebook.com/v9.0/{page.Id}/videos?upload_phase=start&file_size={fileSizeInBytes}")
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };

            var request = new RestRequest(Method.POST);

            try
            {
                var response = _fbRestClient.Execute(request);
                if (!response.IsSuccessful)
                {
                    return FacebookGraphResult<FacebookUploadSession>.Fail(response.Content);
                }


                var uploadSession = JsonConvert.DeserializeObject<FacebookUploadSession>(response.Content);
                return FacebookGraphResult<FacebookUploadSession>.Success(uploadSession);
            }
            catch (Exception ex)
            {
                return FacebookGraphResult<FacebookUploadSession>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// FOR IMMEDIATE PUBLISH
        /// Returns video_id on Facebook
        /// </summary>
        /// <param name="token"></param>
        /// <param name="page"></param>
        /// <param name="pathToVideo"></param>
        /// <returns></returns>
        public FacebookGraphResult<FacebookUploadVideoResponse> UploadVideo(OAuthv2AccessToken token, FacebookPageInformation page, string pathToVideo, string description)
        {
            var _fbRestClient = new RestClient($"https://graph-video.facebook.com/v9.0/{page.Id}/videos")
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };

            var request = new RestRequest(Method.POST);
            request.AddFile("source", pathToVideo);
            request.AddParameter("description", description);

            try
            {
                var response = _fbRestClient.Execute(request);
                if (!response.IsSuccessful)
                {
                    return FacebookGraphResult<FacebookUploadVideoResponse>.Fail(response.Content);
                }
                var uploadVideoResponse = JsonConvert.DeserializeObject<FacebookUploadVideoResponse>(response.Content);
                return FacebookGraphResult<FacebookUploadVideoResponse>.Success(uploadVideoResponse);
            }
            catch (Exception ex)
            {
                return FacebookGraphResult<FacebookUploadVideoResponse>.Fail(ex.Message);
            }
        }

        /// <summary>
        /// FOR SCHEDULED PUBLISH. 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="page"></param>
        /// <param name="pathToVideo"></param>
        /// <param name="description"></param>
        /// <param name="unixTimestamp"></param>
        /// <returns></returns>
        public FacebookGraphResult<FacebookUploadVideoResponse> UploadVideo(OAuthv2AccessToken token, FacebookPageInformation page, string pathToVideo, string description, long unixTimestamp)
        {

            var _fbRestClient = new RestClient($"https://graph-video.facebook.com/v9.0/{page.Id}/videos")
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };

            var request = new RestRequest(Method.POST);
            request.AddFile("source", pathToVideo);
            request.AddParameter("description", description);
            request.AddParameter("published", "false");
            request.AddParameter("scheduled_publish_time", unixTimestamp);

            try
            {
                var response = _fbRestClient.Execute(request);
                if (!response.IsSuccessful)
                {
                    return FacebookGraphResult<FacebookUploadVideoResponse>.Fail(response.Content);
                }
                var uploadVideoResponse = JsonConvert.DeserializeObject<FacebookUploadVideoResponse>(response.Content);
                return FacebookGraphResult<FacebookUploadVideoResponse>.Success(uploadVideoResponse);
            }
            catch (Exception ex)
            {
                return FacebookGraphResult<FacebookUploadVideoResponse>.Fail(ex.Message);
            }
     
        }
        #endregion
    }
}
