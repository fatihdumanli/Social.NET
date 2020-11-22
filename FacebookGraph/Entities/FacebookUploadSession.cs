using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.FacebookGraph.Entities
{
    public class FacebookUploadSession
    {
        [JsonProperty("video_id")]
        public string VideoId { get; set; }
        [JsonProperty("start_offset")]
        public string StartOffset { get; set; }
        [JsonProperty("end_offset")]
        public string EndOffset { get; set; }
        [JsonProperty("upload_session_id")]
        public string UploadSessionId { get; set; }
    }
}
