using System.Net.Http;
using VkLikeSiteBot.Models;



namespace VkLikeSiteBot.Interfaces
{
    public interface IBotTask
    {
        HttpRequestMessage GetVerificationRequest(SiteUserContext user);

        string SuccessState { get; }

        int Type { get; }
    }
}