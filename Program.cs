using VkLikeSiteBot.Models;
using System.Collections.Generic;
using VkLikeSiteBot.Interfaces;
using System.Threading;
using System.Net.Http;
using sharpvk.Types;
using sharpvk;
using System;



namespace VkLikeSiteBot
{
    class Program
    {
        static SiteUserContext siteUser;
        static SiteClient client;
        static ApiClient vkClient;
        static BotSettings settings;


        static void Main(string[] args)
        {
            settings = BotSettings.GetSettings();

            siteUser = new SiteUserContext
            {
                login = settings.Login,
                pass = settings.Pass,
                uid = settings.Uid,
                token = settings.Token,
                host = settings.Host
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
                    {
                        Console.WriteLine($"\n{DateTime.UtcNow}");
                        Console.WriteLine("there is no task");
                        Thread.Sleep(settings.RecieveDelay * 60 * 1000);
                        continue;
                    }

                    foreach (IBotTask task in tasks)
                    {
                        Console.WriteLine($"\n{DateTime.UtcNow}\ntask\n{task.ToString()}");

                        if (task.Type == BotTasks.JoinTask)
                            HandleBotJoinTask(task as BotJoinTask);
                        else if (task.Type == BotTasks.LikeTask)
                            HandleBotLikeTask(task as BotLikeTask);
                        else
                        {
                            Console.WriteLine("status: undefined task type");
                            continue;
                        }

                        Thread.Sleep(settings.CheckDelay * 1000);

                        bool checkResult = client.CheckTask(task);
                        if (!checkResult)
                            Console.WriteLine("status: Task failed");
                        else
                            Console.WriteLine("status: Task completed");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        static private void HandleBotJoinTask(BotJoinTask task)
        {
            vkClient.JoinGroup(Convert.ToInt32(task.groupId));
        }


        static private void HandleBotLikeTask(BotLikeTask task)
        {
            WallPost post = new WallPost
            {
                OwnerId = task.ownerId,
                Id = task.postId,
                Likes = new Likes
                {
                    CanLike = true
                }
            };

            int status = vkClient.AddLikeToPost(post);
            if(status == 0)
                throw new Exception($"cannot like post {task.postUrl}");

            if(!vkClient.Repost(post))
                throw new Exception($"cannot repost post {task.postUrl}");
        }
    }
}
