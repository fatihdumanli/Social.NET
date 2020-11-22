using SocialMediaSharing.BLL.FacebookGraph.Entities;
using SocialMediaSharing.BLL.OAuth;
using SocialMediaSharing.BLL.OAuthv2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.FacebookGraph
{
    public interface IFacebookGraphClient
    {
        #region Identification
        /// <summary>
        /// To get long-lived facebook token. Lasts 60 days.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        FacebookGraphResult<FacebookLongLivedToken> GetLongLivedToken(OAuthv2AccessToken token, string appId, string appSecret);
        /// <summary>
        /// /api/v8.0/me
        /// </summary>
        FacebookGraphResult<Entities.FacebookPublicInformation> GetPublicInformation(OAuthv2AccessToken token, string[] fields);

        /// <summary>
        /// Gets a list of pages managed by user.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        FacebookGraphResult<List<FacebookPageInformation>> GetPageList(OAuthv2AccessToken token);
        #endregion

        #region Publishing Post Immediately
        /// <summary>
        /// Publishes a post on Facebook page (text-only)
        /// https://graph.facebook.com/{page-id}/feed?message=Hello Fans!&access_token={page-access-token}
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        FacebookGraphResult<bool> PublishPost(OAuthv2AccessToken token, FacebookPageInformation account, string content);

        /// <summary>
        /// Publishes a post on Facebook page (text and image)
        /// https://graph.facebook.com/{page-id}/feed?message=Hello Fans!&access_token={page-access-token}
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        FacebookGraphResult<bool> PublishPost(OAuthv2AccessToken token, FacebookPageInformation account, string content, string imageUrl);

        /// <summary>
        /// Publishes a post on Facebook page (text and link)
        /// https://graph.facebook.com/{page-id}/feed?message=Hello Fans!&access_token={page-access-token}
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        FacebookGraphResult<bool> PublishPost(OAuthv2AccessToken token, string content, FacebookPageInformation account, string link);

        /// <summary>
        /// Publishes a post on Facebook page (VIDEO AND CAPTION)
        /// https://graph.facebook.com/{page-id}/feed?message=Hello Fans!&access_token={page-access-token}
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        FacebookGraphResult<bool> PublishPost(OAuthv2AccessToken token, string videoPath, string description, FacebookPageInformation account);

        #endregion

        /// <summary>
        /// To get scheduled posts from Facebook Graph
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        FacebookGraphResult<List<FacebookPost>> GetScheduledPosts(OAuthv2AccessToken token, FacebookPageInformation account);
        #region Publish Scheduled Posts
        /// <summary>
        /// ONLY TEXT.
        /// Used to schedule a post using Facebook Graph API
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        FacebookGraphResult<bool> SchedulePost(OAuthv2AccessToken token, FacebookPageInformation account, string content, long unixTimeStamp);

        /// <summary>
        /// IMAGE-TEXT
        /// Used to schedule a post using Facebook Graph API
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        /// <param name="content"></param>
        /// <param name="imageUrl"></param>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        FacebookGraphResult<bool> SchedulePost(OAuthv2AccessToken token, FacebookPageInformation account, string content, string imageUrl, long unixTimeStamp);

        /// <summary>
        /// LINK-TEXT
        /// Used to schedule a post using Facebook Graph API
        /// </summary>
        /// <param name="token"></param>
        /// <param name="account"></param>
        /// <param name="content"></param>
        /// <param name="imageUrl"></param>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        FacebookGraphResult<bool> SchedulePost(OAuthv2AccessToken token, string link, string content, FacebookPageInformation account, long unixTimeStamp);
        /// <summary>
        /// VIDEO-DESCRIPTION.
        /// Schedules a post on facebook.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="pathToVideo"></param>
        /// <param name="account"></param>
        /// <param name="description"></param>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        FacebookGraphResult<bool> SchedulePost(OAuthv2AccessToken token, string pathToVideo, FacebookPageInformation account, string description, long unixTimeStamp);
        #endregion

        #region Video Upload
        /// <summary>
        /// Starts a upload session on facebook video graph.
        /// https://developers.facebook.com/docs/video-api/guides/publishing
        /// </summary>
        /// <param name="token"></param>
        /// <param name="fileSizeInBytes"></param>
        /// <returns></returns>
        FacebookGraphResult<FacebookUploadSession> StartVideoUploadSession(OAuthv2AccessToken token, FacebookPageInformation page, string fileSizeInBytes);

        /// <summary>
        /// Returns video_id on Facebook
        /// </summary>
        /// <param name="token"></param>
        /// <param name="page"></param>
        /// <param name="pathToVideo"></param>
        /// <returns></returns>
        FacebookGraphResult<FacebookUploadVideoResponse> UploadVideo(OAuthv2AccessToken token, FacebookPageInformation page, string pathToVideo, string description);

        /// <summary>
        /// FOR SCHEDULED PUBLISH
        /// </summary>
        /// <param name="token"></param>
        /// <param name="page"></param>
        /// <param name="pathToVideo"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        FacebookGraphResult<FacebookUploadVideoResponse> UploadVideo(OAuthv2AccessToken token, FacebookPageInformation page, string pathToVideo, string description, long unixTimestamp);
        #endregion

    }
}
