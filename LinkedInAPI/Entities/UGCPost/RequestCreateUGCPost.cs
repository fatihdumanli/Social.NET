using Newtonsoft.Json;
using SocialMediaSharing.BLL.LinkedInAPI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI.Entities.UGCPost
{
    public class RequestCreateUGCPost
    {

        public static RequestCreateUGCPost Create(LinkedInAccount account, string asset, string description)
        {
            var request = new RequestCreateUGCPost();
            request.Author = account.UserId.ToOwnerInfo(account.PersonalOrOrganization);
            request.LifecycleState = "PUBLISHED";
            request.SpecificContent.ComLinkedinUgcShareContent.Media = new List<Medium>();
            request.SpecificContent.ComLinkedinUgcShareContent.Media.Add(new Medium()
            {
                Media = asset,
                Status = "READY",
                Title = new Title() { Text = description }
            });
            request.SpecificContent.ComLinkedinUgcShareContent.ShareCommentary = new ShareCommentary();
            request.SpecificContent.ComLinkedinUgcShareContent.ShareCommentary.Text = description;
            request.SpecificContent.ComLinkedinUgcShareContent.ShareMediaCategory = "VIDEO";         

            request.Visibility = new Visibility();
            request.Visibility.ComLinkedinUgcMemberNetworkVisibility = "PUBLIC";
            return request;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public RequestCreateUGCPost()
        {
            SpecificContent = new SpecificContent();
            TargetAudience = new TargetAudience();
            Visibility = new Visibility();
        }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("lifecycleState")]
        public string LifecycleState { get; set; }

        [JsonProperty("specificContent")]
        public SpecificContent SpecificContent { get; set; }

        [JsonProperty("targetAudience")]
        public TargetAudience TargetAudience { get; set; }

        [JsonProperty("visibility")]
        public Visibility Visibility { get; set; }
    }

    public class Title
    {
        public Title()
        {
            Attributes = new List<object>();
        }

        [JsonProperty("attributes")]
        public List<object> Attributes { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class Description
    {
        public Description()
        {
            Attributes = new List<object>();
        }

        [JsonProperty("attributes")]
        public List<object> Attributes { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
    
    public class Medium
    {
        public Medium()
        {
            Title = new Title();
        }

        [JsonProperty("media")]
        public string Media { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("title")]
        public Title Title { get; set; }

        [JsonProperty("description")]
        public Description Description { get; set; }

        [JsonProperty("originalUrl")]
        public string OriginalUrl { get; set; }
        
    }

    public class ShareCommentary
    {
        public ShareCommentary()
        {
            Attributes = new List<object>();
        }

        [JsonProperty("attributes")]
        public List<object> Attributes { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }

    public class ComLinkedinUgcShareContent
    {
        public ComLinkedinUgcShareContent()
        {
            Media = new List<Medium>();
            ShareCommentary = new ShareCommentary();
        }

        [JsonProperty("media")]
        public List<Medium> Media { get; set; }

        [JsonProperty("shareCommentary")]
        public ShareCommentary ShareCommentary { get; set; }

        [JsonProperty("shareMediaCategory")]
        public string ShareMediaCategory { get; set; }
    }

    public class SpecificContent
    {
        public SpecificContent()
        {
            ComLinkedinUgcShareContent = new ComLinkedinUgcShareContent();
        }

        [JsonProperty("com.linkedin.ugc.ShareContent")]
        public ComLinkedinUgcShareContent ComLinkedinUgcShareContent { get; set; }
    }

    public class TargetedEntity
    {
        public TargetedEntity()
        {
            Locations = new List<string>();
            Seniorities = new List<string>();
        }

        [JsonProperty("locations")]
        public List<string> Locations { get; set; }

        [JsonProperty("seniorities")]
        public List<string> Seniorities { get; set; }
    }

    public class TargetAudience
    {
        public TargetAudience()
        {
            TargetedEntities = new List<TargetedEntity>();
        }

        [JsonProperty("targetedEntities")]
        public List<TargetedEntity> TargetedEntities { get; set; }
    }

    public class Visibility
    {
        [JsonProperty("com.linkedin.ugc.MemberNetworkVisibility")]
        public string ComLinkedinUgcMemberNetworkVisibility { get; set; }
    }

    
}
