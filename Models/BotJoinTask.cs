using VkLikeSiteBot.Interfaces;
using System.Net.Http;
using System.Text;
using System;



namespace VkLikeSiteBot.Models
{
    public class BotJoinTask : IBotTask
    {
        private const string successState = "";

        public string taskId;

        public string groupId;

        public string groupUrl;

        public string api;

        public BotTaskType Type
        {
            get
            {
                return BotTaskType.JoinGroup;
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
            request.RequestUri = new Uri($"{user.Host}/do_company.php");
            request.Method = HttpMethod.Post;

            string content = $"uid={user.Uid}&token={user.Token}&gid={groupId}&id={taskId}&api={api}";
            request.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            return request;
        }


        public HttpRequestMessage GetTaskRefusalRequest(SiteUserContext user)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"{user.Host}/ajax.php");
            request.Method = HttpMethod.Post;

            string content = $"uid={user.Uid}&token={user.Token}&hide_company={groupId}";
            request.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");

            return request;
        }


        public override string ToString()
        {
            return $"TaskId: {taskId}" +
                   $"\nGroupId: {groupId}" +
                   $"\nGroupUrl: {groupUrl}" +
                   $"\nApi: {api}";
        }
    }
}