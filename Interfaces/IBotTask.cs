using System.Net.Http;
using VkLikeSiteBot.Models;



namespace VkLikeSiteBot.Interfaces
{
    public interface IBotTask
    {
        HttpRequestMessage GetVerificationRequest(SiteUserContext user);

        int Type { get; }
    }
}