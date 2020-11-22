using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.TwitterAPI.Entities
{
    public class LaunchVideoUploadResponse
    {
        public string MediaId { get; set; }

        private LaunchVideoUploadResponse() { }
        public static LaunchVideoUploadResponse Create(string mediaId)
        {
            return new LaunchVideoUploadResponse() { MediaId = mediaId };
        }
    }
}
