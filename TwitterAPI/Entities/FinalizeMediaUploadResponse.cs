using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.TwitterAPI.Entities
{
    
    public class TwitterMediaUploadProcessingInfo
    {
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("check_after_secs")]
        public string CheckAfterSecs { get; set; }
    }

    public class FinalizeMediaUploadResponse
    {
        [JsonProperty("media_id")]
        public string MediaId { get; set; }
        [JsonProperty("size")]
        public long Size { get; set; }
        [JsonProperty("processing_info")]
        public TwitterMediaUploadProcessingInfo ProcessingInfo { get; set; }

    }
}
