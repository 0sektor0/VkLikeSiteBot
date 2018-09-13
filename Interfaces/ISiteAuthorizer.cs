using VkLikeSiteBot.Models;



namespace VkLikeSiteBot.Interfaces
{
    public interface ISiteAuthorizer
    {
        SiteUserContext AuthorizeInSite();

        void AuthorizeInVkApp(string authUrl);
    }
}
