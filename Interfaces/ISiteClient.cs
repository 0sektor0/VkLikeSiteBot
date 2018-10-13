using VkLikeSiteBot.Models;
using System.Collections.Generic;



namespace VkLikeSiteBot.Interfaces
{
    public interface ISiteClient
    {
        List<IBotTask> ReciveTasks();

        Result<bool> CheckTask(IBotTask task);

        string RefuseTask(IBotTask task);
    }
}