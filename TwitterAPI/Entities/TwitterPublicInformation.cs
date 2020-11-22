using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.TwitterAPI.Entities
{
    public class TwitterPublicInformation : ITwitterAPIEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }

    }
}
