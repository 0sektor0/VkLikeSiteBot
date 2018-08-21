using System;
using System.Net;
using System.Net.Http;
using VkLikeSiteBot.Interfaces;
using VkLikeSiteBot.Models;



namespace VkLikeSiteBot
{
    public class SiteClient : ISiteClient
    {
        private HttpClient _httpClient;
        private ISiteParser _parser;
        private string _token;
        private string _uid; 

        private string _url = "https://v-like.ru";


        public SiteClient(string token, string uid, ISiteParser parser)
        {
            _httpClient = new HttpClient();
            _parser = parser;
            _token = token;
            _uid = uid;
        }


        public SiteClient(string token, string uid) : this(token, uid, new SiteParser())
        {

        }


        public Result<BotTask> ReciveTask()
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"{_url}/auth.php?uid={_uid}&token={_token}&_={DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");
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

            string content = $"uid={_uid}&token={_token}&gid={task.GroupId}&id={task.TaskId}&api={task.Api}";
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