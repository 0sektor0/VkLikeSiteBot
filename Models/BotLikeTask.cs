using VkLikeSiteBot.Interfaces;
using System.Net.Http;
using System.Text;
using System;



namespace VkLikeSiteBot.Models
{
    public class BotLikeTask : IBotTask
    {
        private string successState = "1";
        public string taskId;
        public int postId;
        public int ownerId;
        public string postUrl;
        public string repost;
        public string type;
        public string api;

        public int Type
        {
            get
            {
                return BotTasks.LikeTask;
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
            request.RequestUri = new Uri($"{user.host}/do_like.php");
            request.Method = HttpMethod.Post;

            string content = $"uid={user.uid}&token={user.token}&id={taskId}&iid={postId}&oid={ownerId}&type={type}&repost={repost}&api={api}";
            request.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");
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
                   $"\nrepost: {repost}" +
                   $"\nApi: {api}";
        }
    }
}