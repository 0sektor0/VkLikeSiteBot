using System.Net.Http;
using VkLikeSiteBot.Models;



namespace VkLikeSiteBot.Interfaces
{
    public interface IBotTask
    {
        HttpRequestMessage GetVerificationRequest(SiteUserContext user);

        HttpRequestMessage GetTaskRefusalRequest(SiteUserContext user);

        string SuccessState { get; }

        BotTaskType Type { get; }
    }
}