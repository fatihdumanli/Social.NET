using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI.Entities
{
    public class LinkedInRegisterUploadResponse
    {
        [JsonProperty("value")]
        public Value Value { get; set; }
    }

    public class Headers
    {
        [JsonProperty("Content-Type")]
        public string ContentType { get; set; }

        [JsonProperty("x-amz-server-side-encryption")]
        public string XAmzServerSideEncryption { get; set; }

        [JsonProperty("x-amz-server-side-encryption-aws-kms-key-id")]
        public string XAmzServerSideEncryptionAwsKmsKeyId { get; set; }
    }

    public class ComLinkedinDigitalmediaUploadingMediaUploadHttpRequest
    {
        [JsonProperty("headers")]
        public Headers Headers { get; set; }

        [JsonProperty("uploadUrl")]
        public string UploadUrl { get; set; }
    }

    public class UploadMechanism
    {
        [JsonProperty("com.linkedin.digitalmedia.uploading.MediaUploadHttpRequest")]
        public ComLinkedinDigitalmediaUploadingMediaUploadHttpRequest ComLinkedinDigitalmediaUploadingMediaUploadHttpRequest { get; set; }
    }

    public class Value
    {
        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("mediaArtifact")]
        public string MediaArtifact { get; set; }

        [JsonProperty("uploadMechanism")]
        public UploadMechanism UploadMechanism { get; set; }
    }

   
}
