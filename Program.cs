using System;
using System.Net;
using System.Net.Http;
using VkLikeSiteBot.Models;



namespace VkLikeSiteBot
{
    class Program
    {
        static void Main(string[] args)
        {
            SiteUserContext siteUser = new SiteUserContext
            {
                login = "+79258465151",
                pass = "YOG_965211-sot",
                uid = "456924527",
                token = "81e50f21fdde8430bcc94c6fc417e9d9"
            };

            SiteParser parser = new SiteParser();
            SiteClient client = new SiteClient(siteUser, parser);

            Result<BotTask> taskResult = client.ReciveTask();
            if (!taskResult.Success)
                throw new Exception(taskResult.Errors[0]);

             Result<bool> checkResult = client.CheckTask(taskResult.Data);
            if (!checkResult.Success)
                throw new Exception(checkResult.Errors[0]);

            //SiteAuthentificator authentificator = new SiteAuthentificator(login, pass);
            //SiteUserContext userContext = authentificator.Authentificate();

            /*CookieContainer _cookieContainer;
            HttpClient _httpClient;

            _cookieContainer = new CookieContainer();

            HttpClientHandler handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                UseCookies = true,
                CookieContainer = _cookieContainer
            };

            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri($"https://v-like.ru/auth.php?uid={uid}&token={token}");

            _httpClient.SendAsync(request).GetAwaiter().GetResult();*/
        }
    }
}
