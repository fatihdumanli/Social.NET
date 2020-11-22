using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.FacebookGraph.Entities
{
    public class FacebookPublicInformation : IFacebookGraphEntity
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
