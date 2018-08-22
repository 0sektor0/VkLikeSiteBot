using System;
using System.Net;
using System.Net.Http;
using VkLikeSiteBot.Interfaces;
using VkLikeSiteBot.Models;



namespace VkLikeSiteBot
{
    public class SiteClient : ISiteClient
    {
        private CookieContainer _cookieContainer;
        private HttpClient _httpClient;
        private ISiteParser _parser;
        private SiteUserContext _user;

        private string _url = "https://v-like.ru";


        public SiteClient(SiteUserContext user, ISiteParser parser)
        {
            _parser = parser;
            _user = user;
            
            HttpClientHandler handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                UseCookies = true,
                CookieContainer = _cookieContainer
            };

            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
        }


        public SiteClient(SiteUserContext user) : this(user, new SiteParser())
        {

        }


        public Result<BotTask> ReciveTask()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"{_url}/auth.php?uid={_user.uid}&token={_user.token}");
            request.Headers.Add("Referer", $"{_url}/");
            request.Headers.Add("Accept", "*/*");
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = _httpClient.SendAsync(request).Result;

            if(response.StatusCode != HttpStatusCode.OK)
                return new Result<BotTask>($"server return {response.StatusCode}");

            string html = response.Content.ReadAsStringAsync().Result;
            return _parser.Parse(response.Content.ReadAsStringAsync().Result);
        }


        public Result<bool> CheckTask(BotTask task)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"{_url}/do_company.php");
            request.Method = HttpMethod.Post;

            string content = $"uid={_user.uid}&token={_user.token}&gid={task.GroupId}&id={task.TaskId}&api={task.Api}";
            request.Content = new StringContent(content);

            HttpResponseMessage response = _httpClient.SendAsync(request).Result;

            if(response.StatusCode != HttpStatusCode.OK)
                return new Result<bool>($"server return {response.StatusCode}");
            else if(response.Content.Headers.ContentLength != 0)
                return new Result<bool>(1, $"{response.Content.ReadAsStringAsync().Result}");

            return new Result<bool>();
        }
    }
}