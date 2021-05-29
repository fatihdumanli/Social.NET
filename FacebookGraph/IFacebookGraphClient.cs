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
      /// https://developers.facebook.com/docs/facebook-login/access-tokens/refreshing/
      /// </summary>
      /// <param name="token"></param>
      /// <param name="appId"></param>
      /// <param name="appSecret"></param>
      /// <returns></returns>
      FacebookGraphResult<FacebookLongLivedToken> GetLongLivedToken(OAuthv2AccessToken token, string appId, string appSecret);

      /// <summary>
      /// For fields: https://developers.facebook.com/docs/graph-api/reference/user/#fields
      /// </summary>
      /// <param name="token"></param>
      /// <param name="fields"></param>
      FacebookGraphResult<Entities.FacebookPublicInformation> GetPublicInformation(OAuthv2AccessToken token, string[] fields);


      /// <summary>
      /// Gets a list of pages managed by user.
      /// </summary>
      /// <param name="token"></param>
      /// <returns></returns>
      FacebookGraphResult<List<FacebookPageInformation>> GetPageList(OAuthv2AccessToken token);

      /// <summary>
      /// To get scheduled posts from Facebook Graph
      /// </summary>
      /// <param name="page"></param>
      /// <returns></returns>
      FacebookGraphResult<List<FacebookPost>> GetScheduledPosts(FacebookPageInformation page);
      #endregion

      #region Publish Post
      /// <summary>
      /// Publishes a post on Facebook page (text-only)
      /// </summary>
      /// <param name="page"></param>
      /// <param name="content"></param>
      /// <returns></returns>
      FacebookGraphResult<FacebookPost> PublishPost(FacebookPageInformation page, string content);

      /// <summary>
      /// Publishes a post on Facebook page (text and image)
      /// </summary>
      /// <param name="page"></param>
      /// <param name="content"></param>
      /// <param name="photo"></param>
      /// <returns></returns>
       FacebookGraphResult<FacebookPost> PublishPost(FacebookPageInformation page, string content, FacebookPhoto photo);

      /// <summary>
      /// Publishes a post on Facebook page (text and link)
      /// </summary>
      /// <param name="page"></param>
      /// <param name="content"></param>
      /// <param name="link"></param>
      /// <returns></returns>
       FacebookGraphResult<FacebookPost> PublishPost(FacebookPageInformation page, string content, string link);

      /// <summary>
      /// Publishes a post on Facebook page (VIDEO AND CAPTION)
      /// </summary>
      /// <param name="page"></param>
      /// <param name="content"></param>
      /// <param name="video"></param>
      /// <returns></returns>
        FacebookGraphResult<FacebookPost> PublishPost(FacebookPageInformation page, string content, FacebookVideo video);
      #endregion

      #region Publish Scheduled Posts
      /// <summary>
      /// ONLY TEXT CONTENT
      /// Schedules a post.
      /// https://developers.facebook.com/docs/pages/publishing/
      /// </summary>
      /// <param name="page"></param>
      /// <param name="content"></param>
      /// <param name="unixTimeStamp"></param>
      /// <returns></returns>
      FacebookGraphResult<FacebookPost> SchedulePost(FacebookPageInformation page, string content, long unixTimeStamp);

      /// <summary>
      /// IMAGE - TEXT
      /// Schedules a post with a photo.
      /// https://developers.facebook.com/docs/pages/publishing/
      /// </summary>
      /// <param name="page"></param>
      /// <param name="content"></param>
      /// <param name="photo"></param>
      /// <param name="unixTimeStamp"></param>
      /// <returns></returns>
      FacebookGraphResult<FacebookPost> SchedulePost(FacebookPageInformation page, string content, FacebookPhoto photo, long unixTimeStamp);

      /// <summary>
      /// LINK - TEXT
      /// Schedules a post with a link attached.
      /// https://developers.facebook.com/docs/pages/publishing/
      /// </summary>
      /// <param name="page"></param>
      /// <param name="link"></param>
      /// <param name="content"></param>
      /// <param name="unixTimeStamp"></param>
      /// <returns></returns>
      FacebookGraphResult<FacebookPost> SchedulePost(FacebookPageInformation page, string link, string content, long unixTimeStamp);

      /// <summary>
      /// VIDEO-DESCRIPTION.
      /// Schedules a post on facebook.
      /// </summary>
      /// <param name="page"></param>
      /// <param name="content"></param>
      /// <param name="video"></param>
      /// <param name="unixTimeStamp"></param>
      /// <returns></returns>
      FacebookGraphResult<FacebookPost> SchedulePost(FacebookPageInformation page, string content, FacebookVideo video, long unixTimeStamp);
      #endregion
   }
}
