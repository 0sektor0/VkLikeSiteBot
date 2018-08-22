using VkLikeSiteBot.Interfaces;
using System.Net.Http;
using System.Text;
using System;



namespace VkLikeSiteBot.Models
{
    public class BotLikeTask : IBotTask
    {
        public string taskId;

        public string postId;

        public string ownerId;

        public string type;

        public string postUrl;

        public string api;

        public int Type
        {
            get
            {
                return BotTasks.JoinTask;
            }
        }


        public HttpRequestMessage GetVerificationRequest(SiteUserContext user)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"{user.host}/do_company.php");
            request.Method = HttpMethod.Post;

            //string content = $"uid={user.uid}&token={user.token}&gid={GroupId}&id={TaskId}&api={Api}";
            //request.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            return request;
        }


        public override string ToString()
        {
            return $"TaskId: {taskId}" +
                   $"\ntype: {type}" +
                   $"\niid: {postId}" +
                   $"\noid: {ownerId}" +
                   $"\nurl: {postUrl}" +
                   $"\nApi: {api}";
        }
    }
}