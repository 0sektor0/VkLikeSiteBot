using System;
using System.Net;
using System.Text;
using System.Net.Http;
using VkLikeSiteBot.Interfaces;
using System.Collections.Generic;
using VkLikeSiteBot.Models;



namespace VkLikeSiteBot.Infrastructure
{
    public class SiteClient
    {
        private SiteParser _parser = new SiteParser();
        private HttpClient _httpClient;
        private SiteUserContext _user;


        public SiteClient(SiteUserContext user)
        {
            _user = user;
            
            HttpClientHandler handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                UseCookies = true,
                CookieContainer = new CookieContainer()
            };

            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        }


        public List<IBotTask> ReciveTask()
        {
            List<IBotTask> tasks = new List<IBotTask>();

            AddToTaskList(tasks, ReciveJoinTask());
            AddToTaskList(tasks, ReciveLikeTask());

            return tasks;
        }


        public void AddToTaskList(List<IBotTask> tasks, IBotTask task)
        {
            if(task != null)
                tasks.Add(task);
        }


        private BotJoinTask ReciveJoinTask()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"{_user.Host}/auth.php?uid={_user.Uid}&token={_user.Token}");
            request.Headers.Add("Referer", $"{_user.Host}/");
            request.Headers.Add("Accept", "*/*");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _httpClient.SendAsync(request).Result;

            string html = response.Content.ReadAsStringAsync().Result;
            return _parser.ParseJoinTask(html);
        }


        private BotLikeTask ReciveLikeTask()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"{_user.Host}/likes.php");
            request.Headers.Add("Referer", $"{_user.Host}/");
            request.Headers.Add("Accept", "*/*");
            request.Method = HttpMethod.Post;

            string content = $"uid={_user.Uid}&token={_user.Token}";
            request.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = _httpClient.SendAsync(request).Result;

            string html = response.Content.ReadAsStringAsync().Result;
            return _parser.ParseLikeTask(html);
        }


        public bool CheckTask(IBotTask task)
        {
            HttpRequestMessage request = task.GetVerificationRequest(_user);
            HttpResponseMessage response = _httpClient.SendAsync(request).Result;

            string result = response.Content.ReadAsStringAsync().Result;
            if(result != task.SuccessState)
                return false;

            return true;
        }
    }
}