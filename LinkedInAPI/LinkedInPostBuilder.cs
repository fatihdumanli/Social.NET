using Newtonsoft.Json.Linq;
using SocialMediaSharing.BLL.LinkedInAPI.Entities;
using SocialMediaSharing.BLL.LinkedInAPI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI
{
    public class LinkedInPostBuilder
    {
        private JObject _post;

        private LinkedInPostBuilder(LinkedInAccount account)
        {
            _post = new JObject();
            _post.Add("owner", account.UserId.ToOwnerInfo(account.PersonalOrOrganization));
        }

        public LinkedInPostBuilder SetText(string text)
        {
            JToken textToken = JToken.FromObject(new { text = text });
            _post.Add("text", textToken);
            return this;
        }

        public LinkedInPostBuilder SetImage(string imageUrl)
        {
            if(string.IsNullOrEmpty(imageUrl))
                return this;

            JObject contentObj = new JObject();
            JArray contentEntities = new JArray();

            JObject contentEntity = new JObject();
            contentEntity["entityLocation"] = imageUrl;
            JArray thumbnailsArray = new JArray();
            JObject thumbnailObj = new JObject();
            thumbnailObj["resolvedUrl"] = imageUrl;
            thumbnailsArray.Add(thumbnailObj);
            contentEntity["thumbnails"] = thumbnailsArray;
            contentEntities.Add(contentEntity);
            contentObj["contentEntities"] = contentEntities;

            _post["content"] = contentObj;
            return this;
        }

        public string Build()
        {
            return _post.ToString();
        }

        public static LinkedInPostBuilder GetInstance(LinkedInAccount account)
        {
            return new LinkedInPostBuilder(account);
        }

        /*
        public LinkedInPostBuilder SetLink(string link)
        {
            _post.Content.ContentEntities[0].EntityLocation = link;
            return this;
        }

        public LinkedInPostBuilder SetImage(string imageUrl)
        {
            _post.Content.ContentEntities[0].Thumbnails[0].ResolvedUrl = imageUrl;
            return this;
        }

        public LinkedInPostBuilder SetTitle(string title)
        {
            _post.Content.Title = title;
            return this;
        }

        public LinkedInPostBuilder SetSubject(string subject)
        {
            _post.Subject = subject;
            return this;
        }

      
        */


    }
}
