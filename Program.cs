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
                catch (VkApiClientException ex)
                {
                    Console.WriteLine($"Vk client exception: {ex.Message}");
                    Thread.Sleep(settings.RecieveDelay * 60 * 1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Thread.Sleep(settings.RecieveDelay * 60 * 1000);
                }
            }
        }


        static private void HandleBotJoinTask(BotJoinTask task)
        {
            vkClient.JoinGroup(Convert.ToInt32(task.groupId));
        }


        static private void HandleBotLikeTask(BotLikeTask task)
        {
            if(task.type == "post")
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

                if (vkClient.AddLikeToPost(post) == 0)
                    throw new Exception($"cannot like post {task.postUrl}");

                if (task.repost == "1")
                    if (!vkClient.Repost(post))
                        throw new Exception($"cannot repost post {task.postUrl}");
            }
            else if(task.type == "photo")
            {
                AttachmentPhoto photo = new AttachmentPhoto
                {
                    OwnerId = task.ownerId,
                    Id = task.postId
                };
                
                if (vkClient.AddLikeToPhoto(photo) == 0)
                    throw new Exception($"cannot like photo {task.postUrl}");
            }
            else
                throw new Exception("undefined task type");
        }
    }
}
