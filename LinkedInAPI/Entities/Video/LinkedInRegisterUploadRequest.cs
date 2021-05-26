using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SocialMediaSharing.BLL.LinkedInAPI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI.Entities
{
    public class LinkedInRegisterUploadRequest
    {

        /// <summary>
        /// Uses an ID to create URN for the owner based on accountType.
        /// Owner in format: urn:li:person:{linkedInUserId}
        /// Owner in format: urn:li:organization:{linkedInUserId}
        /// </summary>
        /// <param name="linkedInUserId"></param>
        /// <param name="accountType"></param>
        /// <returns></returns>
        public static LinkedInRegisterUploadRequest Create(string linkedInUserId, LinkedInAccountType accountType)
        {
            LinkedInRegisterUploadRequest request = new LinkedInRegisterUploadRequest();
            request.RegisterUploadRequest.Owner = linkedInUserId.ToOwnerInfo(accountType);
            request.RegisterUploadRequest.Recipes.Add("urn:li:digitalmediaRecipe:feedshare-video");
            request.RegisterUploadRequest.ServiceRelationships.Add(new ServiceRelationship()
            {
                Identifier = "urn:li:userGeneratedContent",
                RelationshipType = "OWNER"
            });

            return request;
        }

        /// <summary>
        /// Uses URN of the member or organization to create an Upload request
        /// </summary>
        /// <param name="ownerURN"></param>
        /// <returns></returns>
        public static LinkedInRegisterUploadRequest Create(string ownerURN)
        {
            LinkedInRegisterUploadRequest request = new LinkedInRegisterUploadRequest();
            request.RegisterUploadRequest.Owner = ownerURN;
            request.RegisterUploadRequest.Recipes.Add("urn:li:digitalmediaRecipe:feedshare-video");
            request.RegisterUploadRequest.ServiceRelationships.Add(new ServiceRelationship()
            {
                Identifier = "urn:li:userGeneratedContent",
                RelationshipType = "OWNER"
            });

            return request;
        }


        private LinkedInRegisterUploadRequest()
        {
            RegisterUploadRequest = new RegisterUploadRequest();     
        }

        [JsonProperty("registerUploadRequest")]
        public RegisterUploadRequest RegisterUploadRequest { get; set; }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class ServiceRelationship
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("relationshipType")]
        public string RelationshipType { get; set; }
    }

    public class RegisterUploadRequest
    {
        public RegisterUploadRequest()
        {
            ServiceRelationships = new List<ServiceRelationship>();
            Recipes = new List<string>();
        }


        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("recipes")]
        public List<string> Recipes { get; set; }

        [JsonProperty("serviceRelationships")]
        public List<ServiceRelationship> ServiceRelationships { get; set; }
    }

    
}
