using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using SocialMediaSharing.BLL.LinkedInAPI.Entities;
using SocialMediaSharing.BLL.LinkedInAPI.Entities.UGCPost;
using SocialMediaSharing.BLL.LinkedInAPI.Entities.Video;
using SocialMediaSharing.BLL.OAuth;
using SocialMediaSharing.BLL.OAuthv2;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI
{
    public class LinkedInAPIClient : ILinkedInAPIClient
    {
        #region Identification
        /// <summary>
        /// Used to get FirstName, LastName, Id, Profile Picture
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public LinkedInAPIResult<LinkedInPublicInformation> GetLinkedInPublicInformation(OAuthv2AccessToken token, string[] fields)
        {
            var _linkedInRestClient = new RestClient(string.Format("https://api.linkedin.com/v2/me?fields={0}", string.Join(",", fields)))
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };
            var request = new RestRequest(Method.GET);

            try
            {
                var response = _linkedInRestClient.Execute(request);
                if (!response.IsSuccessful)
                {
                    LinkedInAPIResult<LinkedInPublicInformation>.Fail(response.Content);
                }
                var publicInfo = JsonConvert.DeserializeObject<LinkedInPublicInformation>(response.Content);

                return LinkedInAPIResult<LinkedInPublicInformation>.Success(publicInfo);
            }
            catch (Exception ex)
            {
                return LinkedInAPIResult<LinkedInPublicInformation>.Fail(ex.Message);
            }
        }

        #endregion

        #region Share POST
        /// <summary>
        /// FOR TEXT & IMAGE OR ONLY TEXT.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="postInJson"></param>
        /// <returns></returns>
        public LinkedInAPIResult<LinkedInPost> SharePost(OAuthv2AccessToken token, string postInJson)
        {

            var _linkedInRestClient = new RestClient("https://api.linkedin.com/v2/shares")
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };

            var request = new RestRequest(Method.POST);
            request.AddJsonBody(postInJson);

            try
            {
                var response = _linkedInRestClient.Execute(request);
                if (!response.IsSuccessful)
                {
                    return LinkedInAPIResult<LinkedInPost>.Fail(response.Content);
                }

                var resultPost = JsonConvert.DeserializeObject<LinkedInPost>(response.Content);

                return LinkedInAPIResult<LinkedInPost>.Success(resultPost);
            }
            catch (Exception ex)
            {
                return LinkedInAPIResult<LinkedInPost>.Fail(ex.Message);
            }

            throw new NotImplementedException();
        }


        /// <summary>
        /// FOR VIDEO.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        /// <param name="videoPath"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public LinkedInAPIResult<bool> SharePost(OAuthv2AccessToken token, LinkedInAccount account, string videoPath, string description)
        {
            try
            {
                var registerUploadResponse = RegisterVideoUpload(token, LinkedInRegisterUploadRequest.Create(account.UserId, account.PersonalOrOrganization));

                if (!registerUploadResponse.IsSuccessful)
                    return LinkedInAPIResult<bool>.Fail("An error has occured while trying to register video upload on LinkedIn. Details: " + registerUploadResponse.Message);

                var uploadResponse =
                    UploadVideo(token, videoPath, registerUploadResponse.Entity.Value.UploadMechanism.ComLinkedinDigitalmediaUploadingMediaUploadHttpRequest.UploadUrl);

                if (!uploadResponse.IsSuccessful)
                    return LinkedInAPIResult<bool>.Fail("An error has occured while trying to upload video on LinkedIn. Details: " + uploadResponse.Message);

                var createUgcPostResponse = CreateOrganicUGCPost(token, RequestCreateUGCPost.Create(account, registerUploadResponse.Entity.Value.Asset, description));

                if (!createUgcPostResponse.IsSuccessful)
                    return LinkedInAPIResult<bool>.Fail("An error has occured while trying to creating organic UGC post with video on LinkedIn. Details: " + createUgcPostResponse.Message);

                return LinkedInAPIResult<bool>.Success(true);

            }
            catch (Exception ex)
            {
                return LinkedInAPIResult<bool>.Fail("An error has occured while trying publish post with video on LinkedIn. Details: " + ex.Message);
            }
        }
        #endregion

        #region Image (NOT USED)

        /// <summary>
        /// Uploads a media to linkedin 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="uploadUrl"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public LinkedInAPIResult<bool> UploadMedia(OAuthv2AccessToken token, string uploadUrl, string path)
        {
            RestClient restClient = new RestClient(uploadUrl)
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };


            try
            {
                RestRequest restRequest = new RestRequest(Method.POST);
                restRequest.AddHeader("Content-Type", "application/octet-stream");
                restRequest.AddFile("content", path);
                var response = restClient.Execute(restRequest);

                return LinkedInAPIResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return LinkedInAPIResult<bool>.Fail("An error has occured while trying to upload media to LinkedIn. Details: " + ex.Message);
            }
        
        }

        #endregion

        #region Video
        public LinkedInAPIResult<bool> UploadVideo(OAuthv2AccessToken token, string videoPath, string uploadUrl)
        {
            RestClient restClient = new RestClient(uploadUrl)
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };

            try
            {
                RestRequest restRequest = new RestRequest(Method.POST);
                restRequest.AddHeader("Content-Type", "application/octet-stream");
                restRequest.AddFile("upload-file", videoPath);

                var response = restClient.Execute(restRequest);

                return LinkedInAPIResult<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return LinkedInAPIResult<bool>.Fail("An error has occured while trying to upload media to LinkedIn. Details: " + ex.Message);
            }

        }

        public LinkedInAPIResult<LinkedInUploadStatusResponse> CheckUploadStatus(OAuthv2AccessToken token, string asset)
        {
            RestClient restClient = new RestClient($"https://api.linkedin.com/v2/assets/{asset}")
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };
            try
            {
                RestRequest restRequest = new RestRequest(Method.GET);
                var response = restClient.Execute(restRequest);

                var result = JsonConvert.DeserializeObject<LinkedInUploadStatusResponse>(response.Content);
                return LinkedInAPIResult<LinkedInUploadStatusResponse>.Success(result);
            }
            catch (Exception ex)
            {
                return LinkedInAPIResult<LinkedInUploadStatusResponse>.Fail("An error has occured while trying get upload status from LinkedIn. Details: " + ex.Message);
            }
        }

        /// <summary>
        /// LinkedIn register upload
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public LinkedInAPIResult<LinkedInRegisterUploadResponse> RegisterVideoUpload(OAuthv2AccessToken token,
            LinkedInRegisterUploadRequest registerUploadRequestObj)
        {
            var _linkedInRestClient = new RestClient("https://api.linkedin.com/v2/assets?action=registerUpload")
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };

            var request = new RestRequest(Method.POST);
            request.AddJsonBody(registerUploadRequestObj.ToJson());

            try
            {
                var response = _linkedInRestClient.Execute(request);
                if (!response.IsSuccessful)
                {
                    return LinkedInAPIResult<LinkedInRegisterUploadResponse>.Fail(response.Content);
                }

                var registerUploadResponse = JsonConvert.DeserializeObject<LinkedInRegisterUploadResponse>(response.Content);

                return LinkedInAPIResult<LinkedInRegisterUploadResponse>.Success(registerUploadResponse);
            }
            catch (Exception ex)
            {
                return LinkedInAPIResult<LinkedInRegisterUploadResponse>.Fail(ex.Message);
            }

        }


        #endregion

        #region UGCPost
        /// <summary>
        /// 400 	urn:li:developerApplication:{developer application ID} does not have permission to create ugc posts 
        /// Indicates your developer application is not allowlisted to create video UGC Posts. 
        /// UGC Post video creation is restricted to approved applications only.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="createUgcPostRequest"></param>
        /// <returns></returns>
        public LinkedInAPIResult<CreateUGCPostResponse> CreateOrganicUGCPost(OAuthv2AccessToken token, RequestCreateUGCPost createUgcPostRequest)
        {
            var _linkedInRestClient = new RestClient("https://api.linkedin.com/v2/ugcPosts")
            {
                Authenticator = new JwtAuthenticator(token.AccessToken)
            };

            var request = new RestRequest(Method.POST);
            request.AddJsonBody(createUgcPostRequest.ToJson());

            try
            {
                var response = _linkedInRestClient.Execute(request);
                if (!response.IsSuccessful)
                {
                    return LinkedInAPIResult<CreateUGCPostResponse>.Fail(response.Content);
                }
                
                var requestCreateUGCPostResponse = JsonConvert.DeserializeObject<CreateUGCPostResponse>(response.Content);

                return LinkedInAPIResult<CreateUGCPostResponse>.Success(requestCreateUGCPostResponse);
            }
            catch (Exception ex)
            {
                return LinkedInAPIResult<CreateUGCPostResponse>.Fail(ex.Message);
            }
        }

        #endregion
    }
}
