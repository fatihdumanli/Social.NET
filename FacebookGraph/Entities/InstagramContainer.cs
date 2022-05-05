using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaSharing.BLL.FacebookGraph.Entities
{
    // Represents a media container.
    // https://developers.facebook.com/docs/instagram-api/reference/ig-container
    public class InstagramContainer
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("status_code")]
        public InstagramContainerStatusCode StatusCode { get; set; }
    }


    // https://developers.facebook.com/docs/instagram-api/guides/content-publishing#step-1-of-3--create-item-container
    public class InstagramItemContainer
    {
        public bool is_carousel_item { get; set; }
        public string image_url { get; set; }
        public InstagramMediaType media_type { get; set; }
        public string video_url { get; set; }

    }

    /// <summary>
    /// Instagram Carousel Container.
    /// You may publish up to 10 images, videos, or a mix of the two in a single post (a carousel post)
    /// </summary>

    public class InstagramCarouselContainer
    {
        /// <summary>
        /// Set to CAROUSEL. Indicates container is for a carousel.
        /// </summary>
        public InstagramMediaType media_type { get; set; } = InstagramMediaType.CAROUSEL;

        /// <summary>
        /// An array of up to 10 container IDs of each image and video that should appear in the published carousel. Carousels can have up to 10 total images, vidoes, or a mix of the two.
        /// </summary>
        public List<long> children { get; set; }
    }

    public enum InstagramMediaType
    {
        CAROUSEL,
        VIDEO,
        PHOTO
    }
    public enum InstagramContainerStatusCode
    {
        EXPIRED,
        ERROR,
        FINISHED,
        IN_PROGRESS,
        PUBLISHED
    }
}
