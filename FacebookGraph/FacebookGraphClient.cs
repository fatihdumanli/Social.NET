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
      /// To get long-lived facebook token. Lasts 60 days.
      /// https://developers.facebook.com/docs/facebook-login/access-tokens/refreshing/
      /// </summary>
      /// <param name="token"></param>
      /// <param name="appId"></param>
      /// <param name="appSecret"></param>
      /// <returns></returns>
      public FacebookGraphResult<FacebookLongLivedToken> GetLongLivedToken(OAuthv2AccessToken token, string appId, string appSecret)
      {
         var _fbRestClient = new RestClient("https://graph.facebook.com/v10.0/oauth/access_token")
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

      /// <summary>
      /// Gets a list of pages managed by user.
      /// </summary>
      /// <param name="token"></param>
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
      /// To get scheduled posts from Facebook Graph
      /// </summary>
      /// <param name="page"></param>
      /// <returns></returns>
      public FacebookGraphResult<List<FacebookPost>> GetScheduledPosts(FacebookPageInformation page)
      {
         var _fbRestClient = new RestClient($"https://graph.facebook.com/{page.Id}/scheduled_posts")
         {
            Authenticator = new JwtAuthenticator(page.AccessToken)
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
      #endregion

      #region Publish Post
      /// <summary>
      /// Publishes a post on Facebook page (text-only)
      /// </summary>
      /// <param name="page"></param>
      /// <param name="content"></param>
      /// <returns></returns>
      public FacebookGraphResult<FacebookPost> PublishPost(FacebookPageInformation page, string content)
      {
         var _fbRestClient = new RestClient($"https://graph.facebook.com/{page.Id}/feed?message={content}&access_token={page.AccessToken}");

         var request = new RestRequest(Method.POST);

         try
         {
            var response = _fbRestClient.Execute(request);
            if (!response.IsSuccessful)
            {
               return FacebookGraphResult<FacebookPost>.Fail(response.Content);
            }

            return FacebookGraphResult<FacebookPost>.Success(JsonConvert.DeserializeObject<FacebookPost>(response.Content));
         }
         catch (Exception ex)
         {
            return FacebookGraphResult<FacebookPost>.Fail(ex.Message);
         }
      }

      /// <summary>
      /// Publishes a photo post on Facebook page (text and image)
      /// </summary>
      /// <param name="page"></param>
      /// <param name="content"></param>
      /// <param name="imageUrl"></param>
      /// <returns></returns>
      public FacebookGraphResult<FacebookPost> PublishPost(FacebookPageInformation page, string content, FacebookPhoto photo)
      {
         var _fbRestClient = new RestClient($"https://graph.facebook.com/{page.Id}/photos?url={photo.PhotoURL}&published={photo.IsPublished}&message={content}&access_token={page.AccessToken}");

         var request = new RestRequest(Method.POST);

         try
         {
            var response = _fbRestClient.Execute(request);
            if (!response.IsSuccessful)
            {
               return FacebookGraphResult<FacebookPost>.Fail(response.Content);
            }

            var result = JsonConvert.DeserializeObject<FacebookPhoto>(response.Content);

            return FacebookGraphResult<FacebookPost>.Success(new FacebookPost { Id = result.PostID });
         }
         catch (Exception ex)
         {
            return FacebookGraphResult<FacebookPost>.Fail(ex.Message);
         }

      }

      public FacebookGraphResult<FacebookPost> PublishPost(FacebookPageInformation page, string content, List<FacebookPhoto> photos)
      {

         if (photos.Count == 1)
         {
            return PublishPost(page, content, photos[0]);
         }
         else
         {

            List<string> publishedPhotoIds = new List<string>();
            string last_error = "";

            foreach (var photo in photos)
            {
               var _fbRestClient = new RestClient($"https://graph.facebook.com/{page.Id}/photos?published=false&url={photo.PhotoURL}&access_token={page.AccessToken}");
               var request = new RestRequest(Method.POST);

               try
               {
                  var response = _fbRestClient.Execute(request);
                  if (!response.IsSuccessful)
                  {
                     return FacebookGraphResult<FacebookPost>.Fail(response.Content);
                  }

                  var result = JsonConvert.DeserializeObject<FacebookPhoto>(response.Content);

                  publishedPhotoIds.Add(result.Id);
               }
               catch (Exception ex)
               {
                  last_error = ex.Message;
                  // return FacebookGraphResult<FacebookPost>.Fail(ex.Message);
                  // bypassing this image
               }
            }

            if (publishedPhotoIds.Count > 0)
            {
               var _fbRestClient = new RestClient($"https://graph.facebook.com/{page.Id}/feed?message={content}&access_token={page.AccessToken}");

               var request = new RestRequest(Method.POST);

               for (int i = 0; i < publishedPhotoIds.Count; i++)
               {
                  request.AddParameter($"attached_media[{i}]", "{\"media_fbid\":\"" + publishedPhotoIds[i].ToString() + "\"}");
               }

               try
               {
                  var response = _fbRestClient.Execute(request);
                  if (!response.IsSuccessful)
                  {
                     return FacebookGraphResult<FacebookPost>.Fail(response.Content);
                  }

                  return FacebookGraphResult<FacebookPost>.Success(JsonConvert.DeserializeObject<FacebookPost>(response.Content));
               }
               catch (Exception ex)
               {
                  return FacebookGraphResult<FacebookPost>.Fail(ex.Message);
               }
            }
            else
            {
               return FacebookGraphResult<FacebookPost>.Fail("{\"error\":{\"message\":\"All media files failed to upload.\"}}");
            }
         }
      }

      /// <summary>
      /// Publishes a link post on Facebook page/group
      /// </summary>
      /// <param name="page"></param>
      /// <param name="content"></param>
      /// <param name="link"></param>
      /// <returns></returns>
      public FacebookGraphResult<FacebookPost> PublishPost(FacebookPageInformation page, string content, string link)
      {

         var _fbRestClient = new RestClient($"https://graph.facebook.com/{page.Id}/feed?link={link}&message={content}&access_token={page.AccessToken}")
         {
         };

         var request = new RestRequest(Method.POST);

         try
         {
            var response = _fbRestClient.Execute(request);
            if (!response.IsSuccessful)
            {
               return FacebookGraphResult<FacebookPost>.Fail(response.Content);
            }

            return FacebookGraphResult<FacebookPost>.Success(JsonConvert.DeserializeObject<FacebookPost>(response.Content));
         }
         catch (Exception ex)
         {
            return FacebookGraphResult<FacebookPost>.Fail(ex.Message);
         }

      }

      /// <summary>
      /// Publishes a video post on a Facebook page/group
      /// </summary>
      /// <param name="account"></param>
      /// <param name="content"></param>
      /// <param name="video"></param>
      /// <returns></returns>
      public FacebookGraphResult<FacebookPost> PublishPost(FacebookPageInformation page, string content, FacebookVideo video)
      {
         try
         {
            var uploadResult = UploadVideo(page, content, video);

            if (!uploadResult.IsSuccessful)
            {
               return FacebookGraphResult<FacebookPost>.Fail(uploadResult.Message);
            }
            else
            {
               return FacebookGraphResult<FacebookPost>.Success(new FacebookPost { Id = uploadResult.Entity.Id });

            }
         }
         catch (Exception ex)
         {
            return FacebookGraphResult<FacebookPost>.Fail(ex.Message);
         }

      }
      #endregion


      #region Publish Scheduled Facebook Posts
      /// <summary>
      /// ONLY TEXT CONTENT
      /// Schedules a post.
      /// https://developers.facebook.com/docs/pages/publishing/
      /// </summary>
      /// <param name="account"></param>
      /// <param name="content"></param>
      /// <param name="unixTimeStamp"></param>
      /// <returns></returns>
      public FacebookGraphResult<FacebookPost> SchedulePost(FacebookPageInformation account, string content, long unixTimeStamp)
      {
         var _fbRestClient = new RestClient(string.Format("https://graph.facebook.com/{0}/feed?published=false&message={1}&scheduled_publish_time={2}",
                            account.Id, content, unixTimeStamp))
         {
            Authenticator = new JwtAuthenticator(account.AccessToken)
         };

         var request = new RestRequest(Method.POST);

         try
         {
            var response = _fbRestClient.Execute(request);
            if (!response.IsSuccessful)
            {
               return FacebookGraphResult<FacebookPost>.Fail(response.Content);
            }

            return FacebookGraphResult<FacebookPost>.Success(JsonConvert.DeserializeObject<FacebookPost>(response.Content));
         }
         catch (Exception ex)
         {
            return FacebookGraphResult<FacebookPost>.Fail(ex.Message);
         }
      }

      /// <summary>
      /// IMAGE - TEXT
      /// Schedules a post with a photo.
      /// https://developers.facebook.com/docs/pages/publishing/
      /// </summary>
      /// <param name="account"></param>
      /// <param name="content"></param>
      /// <param name="photo"></param>
      /// <param name="unixTimeStamp"></param>
      /// <returns></returns>

      public FacebookGraphResult<FacebookPost> SchedulePost(FacebookPageInformation account, string content, FacebookPhoto photo, long unixTimeStamp)
      {
         var _fbRestClient = new RestClient($"https://graph.facebook.com/{account.Id}/photos?url={photo.PhotoURL}&published=false&message={content}&scheduled_publish_time={unixTimeStamp}")
         {
            Authenticator = new JwtAuthenticator(account.AccessToken)
         };

         var request = new RestRequest(Method.POST);

         try
         {
            var response = _fbRestClient.Execute(request);
            if (!response.IsSuccessful)
            {
               return FacebookGraphResult<FacebookPost>.Fail(response.Content);
            }

            return FacebookGraphResult<FacebookPost>.Success(JsonConvert.DeserializeObject<FacebookPost>(response.Content));
         }
         catch (Exception ex)
         {
            return FacebookGraphResult<FacebookPost>.Fail(ex.Message);
         }
      }

      /// <summary>
      /// LINK - TEXT
      /// Schedules a post with a link attached.
      /// https://developers.facebook.com/docs/pages/publishing/
      /// </summary>
      /// <param name="account"></param>
      /// <param name="link"></param>
      /// <param name="content"></param>
      /// <param name="unixTimeStamp"></param>
      /// <returns></returns>
      public FacebookGraphResult<FacebookPost> SchedulePost(FacebookPageInformation account, string content, string link, long unixTimeStamp)
      {
         var _fbRestClient = new RestClient($"https://graph.facebook.com/{account.Id}/feed?link={link}&published=false&message={content}&scheduled_publish_time={unixTimeStamp}")
         {
            Authenticator = new JwtAuthenticator(account.AccessToken)
         };

         var request = new RestRequest(Method.POST);

         try
         {
            var response = _fbRestClient.Execute(request);
            if (!response.IsSuccessful)
            {
               return FacebookGraphResult<FacebookPost>.Fail(response.Content);
            }

            return FacebookGraphResult<FacebookPost>.Success(JsonConvert.DeserializeObject<FacebookPost>(response.Content));
         }
         catch (Exception ex)
         {
            return FacebookGraphResult<FacebookPost>.Fail(ex.Message);
         }
      }

      /// <summary>
      /// VIDEO-DESCRIPTION. 
      /// Schedules a post.
      /// https://developers.facebook.com/docs/pages/publishing/
      /// </summary>
      /// <param name="page"></param>
      /// <param name="pathToVideo"></param>
      /// <param name="description"></param>
      /// <param name="unixTimeStamp"></param>
      /// <returns></returns>
      public FacebookGraphResult<FacebookPost> SchedulePost(FacebookPageInformation page, string content, FacebookVideo video, long unixTimeStamp)
      {
         var uploadResult = UploadVideo(page, content, video, unixTimeStamp);

         if (!uploadResult.IsSuccessful)
            return FacebookGraphResult<FacebookPost>.Fail(uploadResult.Message);


         var _fbRestClient = new RestClient($"https://graph.facebook.com/{page.Id}/videos")
         {
            Authenticator = new JwtAuthenticator(page.AccessToken)
         };

         var request = new RestRequest(Method.POST);
         request.AddParameter("media_fbid", uploadResult.Entity.Id);

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

            return FacebookGraphResult<FacebookPost>.Success(JsonConvert.DeserializeObject<FacebookPost>(response.Content));
         }
         catch (Exception ex)
         {
            return FacebookGraphResult<FacebookPost>.Fail(ex.Message);
         }
      }

      #endregion

      #region Facebook Video
      /// <summary>
      /// Starts a upload session on facebook video graph.
      /// https://developers.facebook.com/docs/video-api/guides/publishing      
      /// FOR CHUNKED UPLOAD.
      /// </summary>
      /// <param name="page"></param>
      /// <param name="fileSizeInBytes"></param>
      /// <returns></returns>
      private FacebookGraphResult<FacebookUploadSession> StartVideoUploadSession(FacebookPageInformation page, string fileSizeInBytes)
      {
         var _fbRestClient = new RestClient($"https://graph-video.facebook.com/v10.0/{page.Id}/videos?upload_phase=start&file_size={fileSizeInBytes}")
         {
            Authenticator = new JwtAuthenticator(page.AccessToken)
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
      /// <param name="page"></param>
      /// <param name="content"></param>
      /// <param name="video"></param>
      /// <returns></returns>
      private FacebookGraphResult<FacebookVideo> UploadVideo(FacebookPageInformation page, string content, FacebookVideo video)
      {
         var _fbRestClient = new RestClient($"https://graph-video.facebook.com/{page.Id}/videos?file_url={video.VideoURL}&description={content}&published={video.IsPublished}&access_token={page.AccessToken}");

         var request = new RestRequest(Method.POST);

         try
         {
            var response = _fbRestClient.Execute(request);
            if (!response.IsSuccessful)
            {
               return FacebookGraphResult<FacebookVideo>.Fail(response.Content);
            }
            var uploadVideoResponse = JsonConvert.DeserializeObject<FacebookVideo>(response.Content);
            return FacebookGraphResult<FacebookVideo>.Success(uploadVideoResponse);
         }
         catch (Exception ex)
         {
            return FacebookGraphResult<FacebookVideo>.Fail(ex.Message);
         }
      }

      /// <summary>
      /// FOR SCHEDULED PUBLISH. 
      /// </summary>
      /// <param name="page"></param>
      /// <param name="content"></param>
      /// <param name="video"></param>
      /// <param name="unixTimestamp"></param>
      /// <returns></returns>
      private FacebookGraphResult<FacebookVideo> UploadVideo(FacebookPageInformation page, string content, FacebookVideo video, long unixTimestamp)
      {

         var _fbRestClient = new RestClient($"https://graph-video.facebook.com/{page.Id}/videos?file_url={video.VideoURL}&description={content}&published={video.IsPublished}&scheduled_publish_time={unixTimestamp}")
         {
            Authenticator = new JwtAuthenticator(page.AccessToken)
         };

         var request = new RestRequest(Method.POST);

         try
         {
            var response = _fbRestClient.Execute(request);
            if (!response.IsSuccessful)
            {
               return FacebookGraphResult<FacebookVideo>.Fail(response.Content);
            }
            var uploadVideoResponse = JsonConvert.DeserializeObject<FacebookVideo>(response.Content);
            return FacebookGraphResult<FacebookVideo>.Success(uploadVideoResponse);
         }
         catch (Exception ex)
         {
            return FacebookGraphResult<FacebookVideo>.Fail(ex.Message);
         }

      }

      /// <summary>
      /// Returns FB permalinks
      /// </summary>
      public FacebookGraphResult<FacebookPost> GetPostPermalink(FacebookPageInformation page, string post_id)
      {
         var _fbRestClient = new RestClient($"https://graph.facebook.com/{post_id}?fields=permalink_url&access_token={page.AccessToken}");

         var request = new RestRequest(Method.GET);

         try
         {
            var response = _fbRestClient.Execute(request);
            if (!response.IsSuccessful)
            {
               return FacebookGraphResult<FacebookPost>.Fail(response.Content);
            }

            return FacebookGraphResult<FacebookPost>.Success(JsonConvert.DeserializeObject<FacebookPost>(response.Content));
         }
         catch (Exception ex)
         {
            return FacebookGraphResult<FacebookPost>.Fail(ex.Message);
         }
      }
      #endregion

      #region Instagram Posts
      /// <summary>
      /// Supports 1 image or 1 video. Certain rules apply.
      /// https://developers.facebook.com/docs/instagram-api/reference/ig-user/media#creating
      /// </summary>
      /// <param name="page"></param>
      /// <param name="mediaType"></param>
      /// <param name="assetURL"></param>
      /// <param name="caption"></param>
      /// <returns></returns>
      public FacebookGraphResult<InstagramPost> PublishInstagramPost(FacebookPageInformation page, InstagramContainerType mediaType, string assetURL, string caption)
      {
         string last_error = "";
         string containerRequestURL = "";

         if (mediaType == InstagramContainerType.PHOTO)
         {
            containerRequestURL = $"https://graph.facebook.com/{page.Id}/media?image_url={assetURL}&caption={caption}&access_token={page.AccessToken}";
         }
         else if (mediaType == InstagramContainerType.VIDEO)
         {
            containerRequestURL = $"https://graph.facebook.com/{page.Id}/media?media_type=VIDEO&video_url={assetURL}&caption={caption}&access_token={page.AccessToken}";
         }

         var _fbRestClient = new RestClient(containerRequestURL);
         var request = new RestRequest(Method.POST);

         try
         {
            var response = _fbRestClient.Execute(request);
            if (!response.IsSuccessful)
            {
               return FacebookGraphResult<InstagramPost>.Fail(response.Content);
            }

            var result = JsonConvert.DeserializeObject<InstagramContainer>(response.Content);

            var creationId = result.Id;


            if (creationId.Length > 0)
            {
               _fbRestClient = new RestClient($"https://graph.facebook.com/{page.Id}/media_publish?creation_id={creationId}&access_token={page.AccessToken}");

               request = new RestRequest(Method.POST);

               try
               {
                  response = _fbRestClient.Execute(request);
                  if (!response.IsSuccessful)
                  {
                     return FacebookGraphResult<InstagramPost>.Fail(response.Content);
                  }

                  return FacebookGraphResult<InstagramPost>.Success(JsonConvert.DeserializeObject<InstagramPost>(response.Content));
               }
               catch (Exception ex)
               {
                  return FacebookGraphResult<InstagramPost>.Fail(ex.Message);
               }
            }
            else
            {
               return FacebookGraphResult<InstagramPost>.Fail($"Photo upload is failed, {last_error}");
            }

         }
         catch (Exception ex)
         {
            return FacebookGraphResult<InstagramPost>.Fail(ex.Message);
         }
      }

      
      /// <summary>
      /// Returns IG permalink
      /// </summary>
      public FacebookGraphResult<InstagramPost> GetInstagramPostPermalink(FacebookPageInformation page, string post_id)
      {
         var _fbRestClient = new RestClient($"https://graph.facebook.com/{post_id}?fields=permalink&access_token={page.AccessToken}");

         var request = new RestRequest(Method.GET);

         try
         {
            var response = _fbRestClient.Execute(request);
            if (!response.IsSuccessful)
            {
               return FacebookGraphResult<InstagramPost>.Fail(response.Content);
            }

            return FacebookGraphResult<InstagramPost>.Success(JsonConvert.DeserializeObject<InstagramPost>(response.Content));
         }
         catch (Exception ex)
         {
            return FacebookGraphResult<InstagramPost>.Fail(ex.Message);
         }
      }
      #endregion
   }
}
