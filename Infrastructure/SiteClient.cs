using System;
using System.Net;
using System.Text;
using System.Net.Http;
using VkLikeSiteBot.Interfaces;
using System.Collections.Generic;
using VkLikeSiteBot.Models;



namespace VkLikeSiteBot.Infrastructure
{
    public class SiteClient : ISiteClient
    {
        private SiteParser _parser = new SiteParser();
        private Authorizer _authorizer;
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

            _authorizer = new Authorizer(user.Login, user.Pass);
        }


        public List<IBotTask> ReciveTasks()
        {
            List<IBotTask> tasks = new List<IBotTask>();

            AddToTaskList(tasks, ReciveJoinTask());
            AddToTaskList(tasks, ReciveLikeTask());

            return tasks;
        }


        private void AddToTaskList(List<IBotTask> tasks, IBotTask task)
        {
            if (task != null)
                tasks.Add(task);
        }


        private string RecieveTaskPage(string uri, HttpMethod method)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Add("Referer", $"{_user.Host}/");
            request.Headers.Add("Accept", "*/*");
            request.Method = method;

            string content = $"uid={_user.Uid}&token={_user.Token}";
            if (method == HttpMethod.Post)
            {
                request.Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");
                request.RequestUri = new Uri(uri);
            }
            else
                request.RequestUri = new Uri($"{uri}?{content}");

            HttpResponseMessage response = _httpClient.SendAsync(request).Result;
            return response.Content.ReadAsStringAsync().Result;
        }


        private BotJoinTask ReciveJoinTask()
        {
            string html = RecieveTaskPage($"{_user.Host}/auth.php", HttpMethod.Get);
            return _parser.ParseJoinTask(html);
        }


        private BotLikeTask ReciveLikeTask()
        {
            string uri = $"{_user.Host}/likes.php";

            string html = RecieveTaskPage(uri, HttpMethod.Post);
            string authUrl = _parser.ParseAutharization(html);

            if (authUrl != null)
            {
                Console.WriteLine("${DateTime.UtcNow} autharization");
                _authorizer.AuthorizeInVkApp(authUrl);
                html = RecieveTaskPage(uri, HttpMethod.Post);
            }

            return _parser.ParseLikeTask(html);
        }


        //TODO capcha needed
        private BotFriendshipTask RecieveFriendsTaskPage()
        {
            string uri = $"{_user.Host}/friends.php";

            string html = RecieveTaskPage(uri, HttpMethod.Post);
            string authUrl = _parser.ParseAutharization(html);

            if (authUrl != null)
            {
                Console.WriteLine("${DateTime.UtcNow} autharization");
                _authorizer.AuthorizeInVkApp(authUrl);
                html = RecieveTaskPage(uri, HttpMethod.Post);
            }

            html = RecieveTaskPage(uri, HttpMethod.Post);
            return null;
        }


        public Result<bool> CheckTask(IBotTask task)
        {
            HttpRequestMessage request = task.GetVerificationRequest(_user);
            HttpResponseMessage response = _httpClient.SendAsync(request).Result;

            string result = response.Content.ReadAsStringAsync().Result;
            if (result != task.SuccessState)
                return new Result<bool>(result);

            return new Result<bool>(true);
        }


        public string RefuseTask(IBotTask task)
        {
            HttpRequestMessage request = task.GetTaskRefusalRequest(_user);

            if (request == null)
                return "request is empty";

            HttpResponseMessage response = _httpClient.SendAsync(request).Result;
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
    }
}