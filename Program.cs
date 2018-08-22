using VkLikeSiteBot.Models;
using System.Collections.Generic;
using VkLikeSiteBot.Interfaces;
using System.Threading;
using System.Net.Http;
using sharpvk;
using System;



namespace VkLikeSiteBot
{
    class Program
    {
        static SiteUserContext siteUser;
        static SiteClient client;
        static ApiClient vkClient;


        static void Main(string[] args)
        {
            siteUser = new SiteUserContext
            {
                login = "+79258465151",
                pass = "YOG_965211-sot",
                uid = "456924527",
                token = "81e50f21fdde8430bcc94c6fc417e9d9",
                host = "https://v-like.ru"
            };

            client = new SiteClient(siteUser);

            Token t = new Token(siteUser.login, siteUser.pass, 274556);
            vkClient = new ApiClient(t, 3);
            Console.WriteLine($"{siteUser.login} {siteUser.pass} authorized");

            DoWork();
        }


        static void DoWork()
        {
            while (true)
            {
                try
                {
                    List<IBotTask> tasks = client.ReciveTask();

                    if (tasks.Count == 0)
                        throw new Exception("there is no task");

                    foreach (IBotTask task in tasks)
                    {
                        Console.WriteLine($"\ntask\n{task.ToString()}");

                        if (task.Type == BotTasks.JoinTask)
                            HandleBotJoinTask(task as BotJoinTask);
                        else
                        {
                            Console.WriteLine("status: undefined task type");
                            continue;
                        }

                        Thread.Sleep(10 * 1000);

                        Result<bool> checkResult = client.CheckTask(task);
                        if (!checkResult.Success)
                            Console.WriteLine("status: Task failed");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("\n10 min sleep");
                    Thread.Sleep(10 * 60 * 1000);
                }
            }
        }


        static private void HandleBotJoinTask(BotJoinTask task)
        {
            vkClient.JoinGroup(Convert.ToInt32(task.groupId));
        }
    }
}
