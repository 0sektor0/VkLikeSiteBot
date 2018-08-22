namespace VkLikeSiteBot.Models
{
    public class BotTask
    {
        public string TaskId;

        public string GroupId;

        public string GroupUrl;

        public string Api;

        public override string ToString()
        {
            return $"TaskId: {TaskId}" +
                   $"\nGroupId: {GroupId}" +
                   $"\nGroupUrl: {GroupUrl}" +
                   $"\nApi: {Api}";
        }
    }
}