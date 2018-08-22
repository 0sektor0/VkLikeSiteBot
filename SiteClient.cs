using System;
using System.Net;
using System.Text;
using System.Net.Http;
using VkLikeSiteBot.Interfaces;
using System.Collections.Generic;
using VkLikeSiteBot.Models;



namespace VkLikeSiteBot
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

            tasks.Add(ReciveLikeTask());
            //tasks.Add(ReciveJoinTask());

            return tasks;
        }


        private BotJoinTask ReciveJoinTask()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"{_user.host}/auth.php?uid={_user.uid}&token={_user.token}");
            request.Headers.Add("Referer", $"{_user.host}/");
            request.Headers.Add("Accept", "*/*");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _httpClient.SendAsync(request).Result;

            string html = response.Content.ReadAsStringAsync().Result;
            return _parser.ParseJoinTask(html);
        }


        private BotLikeTask ReciveLikeTask()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"{_user.host}/likes.php");
            request.Headers.Add("Referer", $"{_user.host}/");
            request.Headers.Add("Accept", "*/*");
            request.Method = HttpMethod.Post;

            string content = $"uid={_user.uid}&token={_user.token}";
            request.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = _httpClient.SendAsync(request).Result;

            string html = response.Content.ReadAsStringAsync().Result;
            return _parser.ParseLikeTask(html);
        }


        public Result<bool> CheckTask(IBotTask task)
        {
            HttpRequestMessage request = task.GetVerificationRequest(_user);
            HttpResponseMessage response = _httpClient.SendAsync(request).Result;

            if(response.StatusCode != HttpStatusCode.OK)
                return new Result<bool>($"server return {response.StatusCode}");
            else if(response.Content.Headers.ContentLength != 0)
                return new Result<bool>(1, $"{response.Content.ReadAsStringAsync().Result}");

            return new Result<bool>(true);
        }
    }
}