﻿using System;
using System.Net;
using System.Text;
using System.Net.Http;
using VkLikeSiteBot.Models;
using VkLikeSiteBot.Interfaces;
using System.Collections.Generic;
using System.Text.RegularExpressions;



namespace VkLikeSiteBot.Infrastructure
{
    class Authorizer : ISiteAuthorizer
    {
        private CookieContainer _cookieContainer;
        private HttpClient _httpClient;
        private string _login;
        private string _pass;


        public Authorizer(string login, string pass)
        {
            _login = login;
            _pass = pass;

            _cookieContainer = new CookieContainer();

            HttpClientHandler handler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                UseCookies = true,
                CookieContainer = _cookieContainer
            };

            _httpClient = new HttpClient(handler);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.79 Safari/537.36");
            _httpClient.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, image/jxr, */*");
            _httpClient.DefaultRequestHeaders.Add("Accept-Language", "ru-RU");
            _httpClient.DefaultRequestHeaders.Add("Accept-Charset", "utf-8");
            _httpClient.DefaultRequestHeaders.Connection.Add("Keep-Alive");
        }


        private List<KeyValuePair<string, string>> ParseVkAuthFormData(string html)
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


        private FormUrlEncodedContent CreateVkAuthentificatePostData(string authentificateFormHtml)
        {
            List<KeyValuePair<string, string>> postData = ParseVkAuthFormData(authentificateFormHtml);
            postData.Add(new KeyValuePair<string, string>("email", _login));
            postData.Add(new KeyValuePair<string, string>("pass", _pass));

            return new FormUrlEncodedContent(postData);
        }


        private string ParseApprovementUrl(string html)
        {
            Regex regex = new Regex(@"action=\""(.+)"">");
            Match match = regex.Match(html);

            if (!match.Success)
                return "";

            return match.Groups[1].Value;
        }


        private string ParseSiteToken(string html)
        {
            Regex regex = new Regex(@"token = '(.+)'");
            MatchCollection matches = regex.Matches(html);

            if (matches.Count != 1)
                throw new Exception("authentification failed");

            return matches[0].Groups[1].ToString();
        }


        //TODO change auth later
        public void AuthorizeInVkApp(string url)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            HttpResponseMessage response = _httpClient.SendAsync(request).GetAwaiter().GetResult();
            string html = response.Content.ReadAsStringAsync().Result;

            //4th request to authorize on vk
            request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://login.vk.com/?act=login&soft=1&utf8=1");
            request.Content = CreateVkAuthentificatePostData(html);
            request.Method = HttpMethod.Post;

            response = _httpClient.SendAsync(request).Result;
            html = response.Content.ReadAsStringAsync().Result;

            string approvementUrl = ParseApprovementUrl(html);
            if (approvementUrl != "")
            {
                request = new HttpRequestMessage();
                request.RequestUri = new Uri(approvementUrl);

                response = _httpClient.SendAsync(request).Result;
                html = response.Content.ReadAsStringAsync().Result;
            }
        }


        public SiteUserContext AuthorizeInSite()
        {
            //1st request to get session cookie
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://v-like.ru/");

            _httpClient.SendAsync(request).GetAwaiter().GetResult();

            //2nd request to start authentificaation
            request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://ulogin.ru/version/2.0/html/buttons_receiver.html?" +
                "four=https%3A%2F%2Fulogin.ru%2Fauth.php%3Fname%3Dvkontakte%26window%3D1%26lang%3Dru%26fields%3Dfirst_name%2Clast_name%26force_fields%3D%26popup_css%3D%26host%3Dv-like.ru%26optional%3D%26redirect_uri%3Dhttps%253A%252F%252Fv-like.ru%26verify%3D%26callback%3D%26screen%3D2021x1137%26url%3D%26providers%3Dvkontakte%2Cyoutube%26hidden%3Dother%26m%3D0%26page%3Dhttps%253A%252F%252Fv-like.ru%252F%26icons_32%3D%26icons_16%3D%26theme%3Dclassic%26client%3D" +
                "&r=94531" +
                "&xdm_e=https%3A%2F%2Fv-like.ru" +
                "&xdm_c=default6604" +
                "&xdm_p=1");

            _httpClient.SendAsync(request).GetAwaiter().GetResult();

            //3rd request to get authentification form
            request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://ulogin.ru/auth.php?" +
                "name=vkontakte" +
                "&window=1" +
                "&lang=ru" +
                "&fields=first_name,last_name" +
                "&force_fields=" +
                "&popup_css=" +
                "&host=v-like.ru" +
                "&optional=" +
                "&redirect_uri=https%3A%2F%2Fv-like.ru" +
                "&verify=" +
                "&callback=" +
                "&screen=2021x1137" +
                "&url=" +
                "&providers=vkontakte,youtube" +
                "&hidden=other" +
                "&m=0" +
                "&page=https%3A%2F%2Fv-like.ru%2F" +
                "&icons_32=" +
                "&icons_16=" +
                "&theme=classic" +
                "&client=");

            HttpResponseMessage response = _httpClient.SendAsync(request).Result;
            string html = response.Content.ReadAsStringAsync().Result;

            //4th request to authorize on vk
            request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://login.vk.com/?act=login&soft=1");
            request.Content = CreateVkAuthentificatePostData(html);
            request.Method = HttpMethod.Post;

            response = _httpClient.SendAsync(request).Result;
            html = response.Content.ReadAsStringAsync().Result;

            //5th request to get site token
            request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://v-like.ru/");
            request.Headers.Add("Cache-Control", "no-cache");
            request.Headers.Add("Referer", "https://v-like.ru/");
            request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            request.Content = new StringContent($"token={ParseSiteToken(html)}");
            request.Method = HttpMethod.Post;

            response = _httpClient.SendAsync(request).Result;

            return null;
        }
    }
}
