using VkLikeSiteBot.Interfaces;
using System.Net.Http;
using System.Text;
using System;



namespace VkLikeSiteBot.Models
{
    public class BotFriendshipTask : IBotTask
    {
        private string successState = "ok";
        public string taskId;
        public string friendId;

        public BotTaskType Type
        {
            get
            {
                return BotTaskType.Addfriend;
            }
        }

        public string SuccessState
        {
            get
            {
                return successState;
            }
        }


        public HttpRequestMessage GetVerificationRequest(SiteUserContext user)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"{user.Host}/do_like.php");
            request.Method = HttpMethod.Post;

            string content = $"uid={user.Uid}&token={user.Token}&id={taskId}&fuid={friendId}";
            request.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            return request;
        }


        public HttpRequestMessage GetTaskRefusalRequest(SiteUserContext user)
        {
            return null;
        }


        public override string ToString()
        {
            return $"TaskId: {taskId}" +
                   $"\ntype: {friendId}" +
                   $"\nurl: https://vk.com/id{friendId}";
        }
    }
}