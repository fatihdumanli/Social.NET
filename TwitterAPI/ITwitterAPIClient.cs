using SocialMediaSharing.BLL.OAuth;
using SocialMediaSharing.BLL.TwitterAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.TwitterAPI
{
    public interface ITwitterAPIClient
    {
        TwitterAPIResult<TwitterMedia> UploadImage(OAuthv1AccessToken token, byte[] serializedImage);
        TwitterAPIResult<TwitterPublicInformation> GetTwitterPublicInformation(OAuthv1AccessToken token, string screenName);
        TwitterAPIResult<List<Tweet>> GetTweets(OAuthv1AccessToken token, string from);
        TwitterAPIResult<Tweet> PostTweet(OAuthv1AccessToken token, string content);
        TwitterAPIResult<Tweet> PostTweet(OAuthv1AccessToken token, byte[] imageBinary, string textContent);
        TwitterAPIResult<Tweet> PostTweet(OAuthv1AccessToken token, MediaItem mediaItem, string textContent);
        TwitterAPIResult<Tweet> DeleteTweet(OAuthv1AccessToken token, string id);
        TwitterAPIResult<string> InitMediaUpload(OAuthv1AccessToken token, MediaItem media);
        TwitterAPIResult<string> UploadChunk(OAuthv1AccessToken token, string media_id, byte[] chunk, int chunkId);
        TwitterAPIResult<FinalizeMediaUploadResponse> FinalizeMediaUpload(OAuthv1AccessToken token, string media_id);
        TwitterAPIResult<TwitterMediaUploadStatus> GetUploadStatus(OAuthv1AccessToken token, string media_id);
        TwitterAPIResult<LaunchVideoUploadResponse> LaunchVideoUploadChain(OAuthv1AccessToken token, MediaItem video);

    }
}
