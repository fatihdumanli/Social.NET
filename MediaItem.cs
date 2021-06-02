using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL
{
    public enum MediaType
    {
        Image, Video, Link, Text
    }
    
    /// <summary>
    /// Represents a media item that will be published on social media platforms
    /// </summary>
    public class MediaItem
    {
        /// <summary>
        /// For twitter media upload
        /// amplify_video, tweet_image
        /// </summary>
        public string MediaCategory
        {
            get
            {
                return IsVideo ? "amplify_video" : "tweet_image";
            }
        }

        public bool IsVideo
        {
            get
            {
                return MediaType == MediaType.Video;
            }
        }

        public bool IsImage
        {
            get
            {
                return MediaType == MediaType.Image;
            }
        }

        public MediaType MediaType
        {
            get
            {
                if (MimeType.Split('/')[0].Equals("image"))
                    return MediaType.Image;

                else if (MimeType.Split('/')[0].Equals("video"))
                    return MediaType.Video;

                else if (MimeType.Equals("application/octet-stream"))
                    return MediaType.Video;

                else
                    throw new InvalidOperationException("Only image and video is allowed");
            }
        }
        public string MimeType { get; set; }
        public string FileName { get; set; }
        /// <summary>
        /// Full path on server. ie. c:\inetpub\wwwroot\media_uploads\1.mp4
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// The url. ie. http://example.com/media_uploads/1.png
        /// </summary>
        public string Url { get; set; }
        public long TotalBytes { get; set; }
        private MediaItem() { }

        /// <summary>
        /// TODO set MediaType depends on MimeType
        /// </summary>
        /// <param name="mimeType"></param>
        /// <param name="fileName"></param>
        /// <param name="fullPath"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static MediaItem Create(string mimeType, string fileName, string fullPath, string url, long totalBytes)
        {
            return new MediaItem() { FileName = fileName, FullPath = fullPath, MimeType = mimeType, Url = url, TotalBytes = totalBytes };
        }

    }
}
