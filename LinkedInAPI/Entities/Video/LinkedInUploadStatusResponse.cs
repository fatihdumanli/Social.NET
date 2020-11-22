using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI.Entities.Video
{
    public class LinkedInUploadStatusResponse
    {
        [JsonProperty("created")]
        public long Created { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("lastModified")]
        public long LastModified { get; set; }

        [JsonProperty("mediaTypeFamily")]
        public string MediaTypeFamily { get; set; }

        [JsonProperty("recipes")]
        public List<Recipe> Recipes { get; set; }

        [JsonProperty("serviceRelationships")]
        public List<ServiceRelationship> ServiceRelationships { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
    public class Recipe
    {
        [JsonProperty("recipe")]
        public string RecipeStr { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class ServiceRelationship
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; }

        [JsonProperty("relationshipType")]
        public string RelationshipType { get; set; }
    }

   


}
