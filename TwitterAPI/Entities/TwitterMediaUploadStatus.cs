using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.TwitterAPI.Entities
{
    public class Video
    {
        [JsonProperty("video_type")]
        public string VideoType { get; set; }
    }


    public class ProcessingInfo
    {
        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("progress_percent")]
        public int ProgressPercent { get; set; }
    }


    public class TwitterMediaUploadStatus
    {
        public bool IsSucceeded
        {
            get
            {
                return this.ProcessingInfo.State == "succeeded";
            }
        }

        [JsonProperty("media_id")]
        public long MediaId { get; set; }

        [JsonProperty("media_id_string")]
        public string MediaIdString { get; set; }

        [JsonProperty("media_key")]
        public string MediaKey { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("expires_after_secs")]
        public int ExpiresAfterSecs { get; set; }

        [JsonProperty("video")]
        public Video Video { get; set; }

        [JsonProperty("processing_info")]
        public ProcessingInfo ProcessingInfo { get; set; }

    }
}
