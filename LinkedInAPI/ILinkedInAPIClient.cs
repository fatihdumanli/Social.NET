using Newtonsoft.Json.Linq;
using SocialMediaSharing.BLL.LinkedInAPI.Entities;
using SocialMediaSharing.BLL.LinkedInAPI.Entities.UGCPost;
using SocialMediaSharing.BLL.LinkedInAPI.Entities.Video;
using SocialMediaSharing.BLL.OAuthv2;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI
{
   public interface ILinkedInAPIClient
   {
      #region Identification
      LinkedInAPIResult<Entities.LinkedInPublicInformation> GetLinkedInPublicInformation(OAuthv2AccessToken token, string[] fields);
      #endregion

      #region Share Post
      /// <summary>
      /// FOR IMAGE & TEXT OR ONLY TEXT.
      /// </summary>
      /// <param name="token"></param>
      /// <param name="postInJson"></param>
      /// <returns></returns>
      LinkedInAPIResult<LinkedInPost> SharePost(OAuthv2AccessToken token, string postInJson);
      #endregion

      /// <summary>
      /// FOR VIDEO
      /// </summary>
      /// <param name="token"></param>
      /// <param name="account"></param>
      /// <param name="videoPath"></param>
      /// <param name="description"></param>
      /// <returns></returns>
      LinkedInAPIResult<bool> SharePost(OAuthv2AccessToken token, LinkedInAccount account, string videoPath, string description);

      #region Upload a Media File (Image)
      /// <summary>
      /// Uploads a media file to LinkedIn 
      /// </summary>
      /// <param name="token"></param>
      /// <param name="uploadUrl"></param>
      /// <param name="mediaPath"></param>
      /// <returns></returns>
      LinkedInAPIResult<HttpResponseMessage> UploadMedia(OAuthv2AccessToken token, string uploadUrl, string mediaPath);

      /// <summary>
      /// Uploads a media file to LinkedIn 
      /// </summary>
      /// <param name="token"></param>
      /// <param name="uploadUrl"></param>
      /// <param name="bytes"></param>
      /// <returns></returns>
      LinkedInAPIResult<HttpResponseMessage> UploadMedia(OAuthv2AccessToken token, string uploadUrl, byte[] bytes);
      #endregion

      #region VIDEO
      LinkedInAPIResult<LinkedInRegisterUploadResponse> RegisterVideoUpload(OAuthv2AccessToken token, LinkedInRegisterUploadRequest registerUploadRequestObj);

      /// <summary>
      /// Uploads a video file to LinkedIn 
      /// </summary>
      /// <param name="uploadUrl"></param>
      /// <param name="videoPath"></param>
      /// <returns></returns>
      LinkedInAPIResult<HttpResponseMessage> UploadVideo(string uploadUrl, string videoPath);
      
      /// <summary>
      /// Uploads a video file to LinkedIn 
      /// </summary>
      /// <param name="uploadUrl"></param>
      /// <param name="bytes"></param>
      /// <returns></returns>
      LinkedInAPIResult<HttpResponseMessage> UploadVideo(string uploadUrl, byte[] bytes);

      /// <summary>
      /// CHECKS UPLOAD STATUS.
      /// </summary>
      /// <param name="token"></param>
      /// <param name="asset"></param>
      /// <returns></returns>
      LinkedInAPIResult<LinkedInUploadStatusResponse> CheckUploadStatus(OAuthv2AccessToken token, string asset);
      #endregion
      #region UGC Post
      LinkedInAPIResult<CreateUGCPostResponse> CreateOrganicUGCPost(OAuthv2AccessToken token, RequestCreateUGCPost request);
      #endregion
   }
}
