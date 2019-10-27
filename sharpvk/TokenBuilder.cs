using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;



namespace SharpVK
{
    public partial class TokenBuilder
    {
        public static Token BuildToken(string login, string password, int appId, int scope)
        {
            HttpClient httpClient = GetHttpClient();
            string url = "https://oauth.vk.com/authorize?client_id={appid}&" +
                      "redirect_uri=https://oauth.vk.com/blank.html&" +
                      "scope={scope}&" +
                      "response_type=token&" +
                      "v=5.53&display=wap";
            
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            HttpResponseMessage response = httpClient.SendAsync(request).GetAwaiter().GetResult();
            string html = response.Content.ReadAsStringAsync().Result;

            //4th request to authorize on vk
            request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://login.vk.com/?act=login&soft=1&utf8=1");

            response = httpClient.SendAsync(request).Result;
            html = response.Content.ReadAsStringAsync().Result;

            string approvementUrl = ParseApprovementUrl(html);
            if (approvementUrl != "")
            {
                request = new HttpRequestMessage();
                request.RequestUri = new Uri(approvementUrl);

                response = httpClient.SendAsync(request).Result;
                html = response.Content.ReadAsStringAsync().Result;
            }

            return null;
        }

        private static HttpClient GetHttpClient()
        {
            HttpClientHandler handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                UseCookies = true,
                CookieContainer = new CookieContainer(),
            };

            HttpClient httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36");
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, image/jxr, */*");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "ru-RU");
            httpClient.DefaultRequestHeaders.Add("Accept-Charset", "utf-8");
            httpClient.DefaultRequestHeaders.Connection.Add("Keep-Alive");

            return httpClient;
        }
        
        private static List<KeyValuePair<string, string>> ParseVkAuthFormData(string html)
        {
            Regex regex = new Regex(@"name=""(.+)"" value=""(.+)""");
            MatchCollection matches = regex.Matches(html);

            if (matches.Count == 0)
                throw new Exception("Form dosent contain any valid data");

            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();

            foreach (Match match in matches)
                data.Add(new KeyValuePair<string, string>(match.Groups[1].Value, match.Groups[2].Value));

            return data;
        }

        private static FormUrlEncodedContent CreateVkAuthentificatePostData(string authentificateFormHtml, string login, string pass)
        {
            List<KeyValuePair<string, string>> postData = ParseVkAuthFormData(authentificateFormHtml);
            postData.Add(new KeyValuePair<string, string>("email", login));
            postData.Add(new KeyValuePair<string, string>("pass", pass));

            return new FormUrlEncodedContent(postData);
        }

        private static string ParseApprovementUrl(string html)
        {
            Regex regex = new Regex(@"action=\""(.+)"">");
            Match match = regex.Match(html);

            if (!match.Success)
                return "";

            return match.Groups[1].Value;
        }

        private static void UpdateToken(Token token)
        {
            
        }
    }
}