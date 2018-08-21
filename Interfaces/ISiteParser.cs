using VkLikeSiteBot.Models;


namespace VkLikeSiteBot.Interfaces
{
    public interface ISiteParser
    {
        Result<BotTask> Parse(string html);
    }
}