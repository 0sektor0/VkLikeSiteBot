using VkLikeSiteBot.Models;



namespace VkLikeSiteBot.Interfaces
{
    public interface ISiteClient
    {
        Result<BotTask> ReciveTask();

        Result<bool> CheckTask(BotTask botTask);
    }
}