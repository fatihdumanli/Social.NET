﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI.Entities
{
    public class LinkedInText
    {
        public LinkedInText(string text)
        {
            Text = text;
        }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
