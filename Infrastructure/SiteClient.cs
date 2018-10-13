/**
    @file       SiteClient.cs
    @brief      Файл с классом SiteClient
    @author     Григорьев Е.А.
    @date       17-07-2018
    @version    1.13.37
\par Использует классы: 
-   @ref        SiteParser
-   @ref        Authorizer
-   @ref        SiteUserContext
-   @ref        IBotTask
-   @ref        BotJoinTask
-   @ref        BotLikeTask
-   @ref        BotFriendshipTask
\par Содержит классы:
-   @ref        SiteClient
*/
using System;
using System.Net;
using System.Text;
using System.Net.Http;
using VkLikeSiteBot.Interfaces;
using System.Collections.Generic;
using VkLikeSiteBot.Models;



namespace VkLikeSiteBot.Infrastructure
{
    /**
        @brief Класс для внешнего взаимодействия с сайтом поставщика

        Данный класс предоставляет рычаги для простого взаимодействия с калссом поставщика
    */
    public class SiteClient : ISiteClient
    {
        private SiteParser _parser = new SiteParser();  ///< Парсер данных сайта
        private Authorizer _authorizer;                 ///< Класс для проведения авторизации
        private HttpClient _httpClient;                 ///< Класс для отправки http запросов
        private SiteUserContext _user;                  ///< Информация о пользователе сайта


        /**
            @brief  конструктор
            @param  Информация о пользователе сайта
         */
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

        /**
            @brief  Получение задач с сайта поставщика
            @return Список задач с сайта
         */
        public List<IBotTask> ReciveTasks()
        {
            List<IBotTask> tasks = new List<IBotTask>();

            AddToTaskList(tasks, ReciveJoinTask());
            AddToTaskList(tasks, ReciveLikeTask());

            return tasks;
        }

        /**
            @brief  Добавить задачу в список задач
            @param  Список куда добавлять задачи
            @param Задача, которую нужно добавить в список
         */
        private void AddToTaskList(List<IBotTask> tasks, IBotTask task)
        {
            if (task != null)
                tasks.Add(task);
        }

        /**
            @brief  Получение страницы с задачами
            @param  урл страницы
            @param  http метод
            @return html страницы
         */
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

        /**
            @brief  Получение задачи на вступление
            @return Задача на вступление
         */
        private BotJoinTask ReciveJoinTask()
        {
            string html = RecieveTaskPage($"{_user.Host}/auth.php", HttpMethod.Get);
            return _parser.ParseJoinTask(html);
        }

        /**
            @brief  Получение задачи на лайк
            @return Задача на лайк
         */
        private BotLikeTask ReciveLikeTask()
        {
            string uri = $"{_user.Host}/likes.php";

            string html = RecieveTaskPage(uri, HttpMethod.Post);
            string authUrl = _parser.ParseAutharization(html);

            if (authUrl != null)
            {
                Console.WriteLine($"{DateTime.UtcNow} autharization");
                _authorizer.AuthorizeInVkApp(authUrl);
                html = RecieveTaskPage(uri, HttpMethod.Post);
            }

            return _parser.ParseLikeTask(html);
        }

        /**
            @brief  Получение задачи на добавление в друзья
            @return Задачи на добавление в друзья
         */
        private BotFriendshipTask RecieveFriendsTaskPage()
        {
            string uri = $"{_user.Host}/friends.php";

            string html = RecieveTaskPage(uri, HttpMethod.Post);
            string authUrl = _parser.ParseAutharization(html);

            if (authUrl != null)
            {
                Console.WriteLine($"{DateTime.UtcNow} autharization");
                _authorizer.AuthorizeInVkApp(authUrl);
                html = RecieveTaskPage(uri, HttpMethod.Post);
            }

            html = RecieveTaskPage(uri, HttpMethod.Post);
            return null;
        }

        /**
            @brief  Проверка стаатуса задачи
            @param  Задача
            @return Результат проверки
         */
        public Result<bool> CheckTask(IBotTask task)
        {
            HttpRequestMessage request = task.GetVerificationRequest(_user);
            HttpResponseMessage response = _httpClient.SendAsync(request).Result;

            string result = response.Content.ReadAsStringAsync().Result;
            if (result != task.SuccessState)
                return new Result<bool>(result);

            return new Result<bool>(true);
        }

        /**
            @brief  Отказ от задачи
            @param  Задача
            @return Строка с ответом
         */
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