using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.LinkedInAPI.Extensions
{
    public static class AssetExtensions
    {
        public static string ToPureAssetId(this string assetId)
        {
            return assetId.Split(':')[3];
        }
    }
}
