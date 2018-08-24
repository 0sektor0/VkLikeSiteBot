using VkLikeSiteBot.Models;



namespace VkLikeSiteBot.Interfaces
{
    public interface ISiteClient
    {
        Result<BotJoinTask> ReciveTask();

        Result<bool> CheckTas(IBotTask botTask);
    }
}