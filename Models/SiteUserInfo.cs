using System.Net.Http;



namespace VkLikeSiteBot
{
    public class SiteUserContext
    {
        public string login { get; }

        public string pass { get;  }

        public string uid { get; }

        public string token { get; }

        public HttpClient httpClient { get; }
    }
}
