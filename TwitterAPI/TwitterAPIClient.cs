using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using SocialMediaSharing.BLL.Logging;
using SocialMediaSharing.BLL.OAuth;
using SocialMediaSharing.BLL.TwitterAPI.Entities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.TwitterAPI
{
    public class TwitterAPIClient : ITwitterAPIClient
    {
        readonly IAPILibLogger _logger;

        public TwitterAPIClient()
        {

        }

        public TwitterAPIClient(IAPILibLogger logger)
        {
            _logger = logger;
        }
        readonly string ConsumerKey = ConfigurationManager.AppSettings["TwitterConsumerKey"];
        readonly string ConsumerSecret = ConfigurationManager.AppSettings["TwitterConsumerSecret"];

        #region Identification

        /// <summary>
        /// Gets profile public information
        /// </summary>
        /// <returns></returns>
        public TwitterAPIResult<TwitterPublicInformation> GetTwitterPublicInformation(OAuthv1AccessToken token, string screenName)
        {
            var _twitterRestClient = new RestClient(string.Format("https://api.twitter.com/1.1/users/show.json"))
            {
                Authenticator = OAuth1Authenticator
                    .ForProtectedResource(ConsumerKey, ConsumerSecret, token.OAuthToken, token.OAuthSecret)
            };

            var request = new RestRequest(Method.GET);
            request.AddParameter("screen_name", screenName);
            
            try
            {
                var response = _twitterRestClient.Execute(request);

                if (!response.IsSuccessful)
                {
                    TwitterAPIResult<TwitterPublicInformation>.Fail(response.Content);
                }

                var entity = JsonConvert.DeserializeObject<TwitterPublicInformation>(response.Content);

                return TwitterAPIResult<Entities.TwitterPublicInformation>.Success(entity);
            }
            catch (Exception ex)
            {
                return TwitterAPIResult<Entities.TwitterPublicInformation>.Fail(ex.Message);
            }

        }

        #endregion

        #region Post status update (tweet)

        /// <summary>
        /// ONLY TEXT.
        /// Posts a tweet. No need user id. That info wrapped in the OAuthv1AccessToken.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public TwitterAPIResult<Tweet> PostTweet(OAuthv1AccessToken token, string content)
        {

            var _twitterRestClient = new RestClient("https://api.twitter.com/1.1/statuses/update.json")
            {
                Authenticator = OAuth1Authenticator
                  .ForProtectedResource(ConsumerKey, ConsumerSecret, token.OAuthToken, token.OAuthSecret)
            };

            var request = new RestRequest(Method.POST);
            request.AddParameter("status", content);

            try
            {
                var response = _twitterRestClient.Execute(request);

                if (!response.IsSuccessful)
                {
                    return TwitterAPIResult<Tweet>.Fail(response.Content);
                }

                var entity = JsonConvert.DeserializeObject<Tweet>(response.Content);

                return TwitterAPIResult<Entities.Tweet>.Success(entity);
            }
            catch (Exception ex)
            {
                return TwitterAPIResult<Entities.Tweet>.Fail(ex.Message);
            }

        }

        /// <summary>
        /// WITH IMAGE & TEXT.
        /// Posts a tweet.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="imageBinary"></param>
        /// <param name="textContent"></param>
        /// <returns></returns>
        public TwitterAPIResult<Tweet> PostTweet(OAuthv1AccessToken token, byte[] imageBinary, string textContent)
        {

            var media = UploadImage(token, imageBinary);

            if(!media.IsSuccessful)
            {
                return TwitterAPIResult<Tweet>.Fail("An error has occured when trying to upload media. Details: " + media.Message);
            }

            var _twitterRestClient = new RestClient("https://api.twitter.com/1.1/statuses/update.json")
            {
                Authenticator = OAuth1Authenticator
                .ForProtectedResource(ConsumerKey, ConsumerSecret, token.OAuthToken, token.OAuthSecret)
            };

            var request = new RestRequest(Method.POST);
            request.AddParameter("status", textContent);
            request.AddParameter("media_ids", media.Entity.MediaId);

            try
            {
                var response = _twitterRestClient.Execute(request);

                if (!response.IsSuccessful)
                {
                    return TwitterAPIResult<Tweet>.Fail(response.Content);
                }

                var entity = JsonConvert.DeserializeObject<Tweet>(response.Content);

                return TwitterAPIResult<Entities.Tweet>.Success(entity);
            }
            catch (Exception ex)
            {
                return TwitterAPIResult<Entities.Tweet>.Fail(ex.Message);
            }

        }

        /// <summary>
        /// WITH VIDEO & TEXT.
        /// Posts a tweet.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="videoFullPath"></param>
        /// <param name="textContent"></param>
        /// <returns></returns>
        public TwitterAPIResult<Tweet> PostTweet(OAuthv1AccessToken token, MediaItem mediaItem, string textContent)
        {

            _logger.LogInfo("-- Started posting tweet with video.");

            var media = LaunchVideoUploadChain(token, mediaItem);

            if (!media.IsSuccessful)
            {
                return TwitterAPIResult<Tweet>.Fail("An error has occured when trying to upload media. Details: " + media.Message);
            }

            //Finalize media uplod with media_id
            var finalizeResult = FinalizeMediaUpload(token, media.Entity.MediaId);

            if (!finalizeResult.IsSuccessful)
                return TwitterAPIResult<Tweet>.Fail("An error has occured while trying to finalize chunked upload. Details: " + finalizeResult.Message);

            var uploadStatus = GetUploadStatus(token, media.Entity.MediaId);

            _logger.LogInfo("-- UploadStatus: " + JsonConvert.SerializeObject(uploadStatus));
       
            if(uploadStatus.Entity == null)
                return TwitterAPIResult<Tweet>.Fail("An error has occured while trying to get the upload status. Details: " + uploadStatus.Message);

            while (!uploadStatus.Entity.IsSucceeded && uploadStatus.Entity != null)
            {
                Thread.Sleep(2000);
                uploadStatus = GetUploadStatus(token, media.Entity.MediaId);

                if (uploadStatus.Entity == null)
                    return TwitterAPIResult<Tweet>.Fail("An error has occured while trying to get the upload status. Details: " + uploadStatus.Message);
            }

            var _twitterRestClient = new RestClient("https://api.twitter.com/1.1/statuses/update.json")
            {
                Authenticator = OAuth1Authenticator
                .ForProtectedResource(ConsumerKey, ConsumerSecret, token.OAuthToken, token.OAuthSecret)
            };

            var request = new RestRequest(Method.POST);
            request.AddParameter("status", textContent);
            request.AddParameter("media_ids", media.Entity.MediaId); //media.Entity is the media_id

            int tryCount = 0;

            try
            {
                var response = _twitterRestClient.Execute(request);


                while (!response.IsSuccessful && tryCount < 5)
                {
                    response = _twitterRestClient.Execute(request);
                    tryCount++;
                }

                if (!response.IsSuccessful)
                {
                    return TwitterAPIResult<Tweet>.Fail(response.Content);
                }

                var entity = JsonConvert.DeserializeObject<Tweet>(response.Content);

                return TwitterAPIResult<Entities.Tweet>.Success(entity);
            }
            catch (Exception ex)
            {
                return TwitterAPIResult<Entities.Tweet>.Fail(ex.Message);
            }

        }
        #endregion

        #region Media
        public TwitterAPIResult<TwitterMedia> UploadImage(OAuthv1AccessToken token, byte[] file)
        {
            var _twitterRestClient = new RestClient("https://upload.twitter.com/1.1/media/upload.json")
            {
                Authenticator = OAuth1Authenticator
                .ForProtectedResource(ConsumerKey, ConsumerSecret, token.OAuthToken, token.OAuthSecret)
            };

            var request = new RestRequest(Method.POST);
            request.AddFile("media", file, "mymedia.png");

            try
            {
                var response = _twitterRestClient.Execute(request);

                if (!response.IsSuccessful)
                {
                    return TwitterAPIResult<TwitterMedia>.Fail(response.Content);
                }

                var entity = JsonConvert.DeserializeObject<TwitterMedia>(response.Content);

                return TwitterAPIResult<TwitterMedia>.Success(entity);
            }
            catch (Exception ex)
            {
                return TwitterAPIResult<TwitterMedia>.Fail(ex.Message);
            }

        }
        /// <summary>
        /// Initiates a new media upload session, returns media_id.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="media"></param>
        /// <returns></returns>
        public TwitterAPIResult<string> InitMediaUpload(OAuthv1AccessToken token, MediaItem media)
        {
            var _twitterRestClient = new RestClient("https://upload.twitter.com/1.1/media/upload.json")
            {
                Authenticator = OAuth1Authenticator
               .ForProtectedResource(ConsumerKey, ConsumerSecret, token.OAuthToken, token.OAuthSecret)
            };

            var request = new RestRequest(Method.POST);
            request.AddParameter("media_type", media.MimeType);
            request.AddParameter("command", "INIT");
            request.AddParameter("total_bytes", media.TotalBytes);
            request.AddParameter("media_category", media.MediaCategory);

            try
            {
                var response = _twitterRestClient.Execute(request);

                if (!response.IsSuccessful)
                {
                    return TwitterAPIResult<string>.Fail(response.Content);
                }

                JObject jObj = JObject.Parse(response.Content);


                return TwitterAPIResult<string>.Success(jObj["media_id"].ToString());
            }
            catch (Exception ex)
            {
                return TwitterAPIResult<string>.Fail(ex.Message);
            }
        }
        public TwitterAPIResult<FinalizeMediaUploadResponse> FinalizeMediaUpload(OAuthv1AccessToken token, string media_id)
        {
            var _twitterRestClient = new RestClient("https://upload.twitter.com/1.1/media/upload.json")
            {
                Authenticator = OAuth1Authenticator
                .ForProtectedResource(ConsumerKey, ConsumerSecret, token.OAuthToken, token.OAuthSecret)
            };

            var request = new RestRequest(Method.POST);
            request.AddParameter("media_id", media_id);
            request.AddParameter("command", "FINALIZE");
           
            try
            {
                var response = _twitterRestClient.Execute(request);

                if (!response.IsSuccessful)
                {
                    return TwitterAPIResult<FinalizeMediaUploadResponse>.Fail(response.Content);
                }

                var finalizeResult = JsonConvert.DeserializeObject<FinalizeMediaUploadResponse>(response.Content);
                return TwitterAPIResult<FinalizeMediaUploadResponse>.Success(finalizeResult);
            }
            catch (Exception ex)
            {
                return TwitterAPIResult<FinalizeMediaUploadResponse>.Fail(ex.Message);
            }

        }
        public TwitterAPIResult<TwitterMediaUploadStatus> GetUploadStatus(OAuthv1AccessToken token, string media_id)
        {

            var _twitterRestClient = new RestClient("https://upload.twitter.com/1.1/media/upload.json")
            {
                Authenticator = OAuth1Authenticator
               .ForProtectedResource(ConsumerKey, ConsumerSecret, token.OAuthToken, token.OAuthSecret)
            };

            var request = new RestRequest(Method.GET);
            request.AddQueryParameter("command", "STATUS");
            request.AddQueryParameter("media_id", media_id);

            try
            {
                var response = _twitterRestClient.Execute(request);

                if (!response.IsSuccessful)
                {
                    return TwitterAPIResult<TwitterMediaUploadStatus>.Fail(response.Content);
                }

                var uploadStatusResult = JsonConvert.DeserializeObject<TwitterMediaUploadStatus>(response.Content);
                _logger.LogInfo("-- UploadStatusResult:"+ response.Content);
                return TwitterAPIResult<TwitterMediaUploadStatus>.Success(uploadStatusResult);
            }
            catch (Exception ex)
            {
                return TwitterAPIResult<TwitterMediaUploadStatus>.Fail(ex.Message);
            }
        }


        public TwitterAPIResult<string> UploadChunk(OAuthv1AccessToken token, string media_id, byte[] chunk, int chunkId)
        {           
            var _twitterRestClient = new RestClient("https://upload.twitter.com/1.1/media/upload.json")
            {
                Authenticator = OAuth1Authenticator
                    .ForProtectedResource(ConsumerKey, ConsumerSecret, token.OAuthToken, token.OAuthSecret)
            };

            string videoBase64 = Convert.ToBase64String(chunk);

            var request = new RestRequest(Method.POST);
            request.AddParameter("media_id", media_id);
            request.AddParameter("command", "APPEND");
            request.AddParameter("segment_index", chunkId);
            request.AddParameter("media_data", videoBase64);

            try
            {
                var response = _twitterRestClient.Execute(request);

                if (!response.IsSuccessful)
                {
                    return TwitterAPIResult<string>.Fail(response.Content);
                }

                return TwitterAPIResult<string>.Success("1");
            }
            catch (Exception ex)
            {
                return TwitterAPIResult<string>.Fail(ex.Message);
            }


        }


        /// <summary>
        /// 1. INIT
        /// 2. UPLOAD BY CHUNKS
        /// 3. FINALIZE
        /// </summary>
        /// <param name="token"></param>
        /// <param name="video"></param>
        /// <returns></returns>
        public TwitterAPIResult<LaunchVideoUploadResponse> LaunchVideoUploadChain(OAuthv1AccessToken token, MediaItem video)
        {
            const int chunkSize = 40 * 1024;
          
            var initStatus = InitMediaUpload(token, video);

            if (!initStatus.IsSuccessful)
                return TwitterAPIResult<LaunchVideoUploadResponse>.Fail("An error has occured while trying to initiate upload media. Details: " + initStatus.Message);

            var media_id = initStatus.Entity;

            using (var file = File.OpenRead(video.FullPath))
            {
                int bytesRead, chunkID = 0;
                var buffer = new byte[chunkSize];


                while ((bytesRead = file.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (bytesRead < chunkSize)
                    {
                        var lastBuffer = new byte[bytesRead];
                        Buffer.BlockCopy(buffer, 0, lastBuffer, 0, bytesRead);
                        buffer = new byte[bytesRead];
                        buffer = lastBuffer;
                    }
                    try
                    {
                        var rezAppend = Task.Run(() =>
                        {
                            var response = UploadChunk(token, media_id, buffer, chunkID);
                            return response;
                        }).Result;

                        if (!rezAppend.IsSuccessful)
                        {
                            return TwitterAPIResult<LaunchVideoUploadResponse>.Fail(rezAppend.Message);
                        }
                    }
                    catch (Exception ex)
                    {
                        return TwitterAPIResult<LaunchVideoUploadResponse>.Fail(ex.Message);
                    }
                    chunkID++;
                }
            }
            

            return TwitterAPIResult<LaunchVideoUploadResponse>.Success(LaunchVideoUploadResponse.Create(media_id));
        }


        #endregion

    }
}
