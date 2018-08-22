using System;
using VkLikeSiteBot.Models;
using System.Threading;
using sharpvk;



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
                token = "81e50f21fdde8430bcc94c6fc417e9d9"
            };

            SiteParser parser = new SiteParser();
            client = new SiteClient(siteUser, parser);
            
            Token t = new Token(siteUser.login, siteUser.pass, 274556);
            vkClient = new ApiClient(t,3);
            Console.WriteLine($"{siteUser.login} {siteUser.pass} authorized");

            DoWork();
        }


        static void DoWork()
        {
            while(true)
            {
                try
                {
                    Result<BotTask> taskResult = client.ReciveTask();
                    if (!taskResult.Success)
                        throw new Exception(taskResult.Errors[0]);

                    BotTask task = taskResult.Data;
                    Console.WriteLine($"\nnew task\n{task.ToString()}\n");

                    int status = vkClient.JoinGroup(Convert.ToInt32(task.GroupId));
                    Console.WriteLine($"groups joined: {status}");
                    Thread.Sleep(10 * 1000);             

                    Result<bool> checkResult = client.CheckTask(task);
                    if (!checkResult.Success)
                        throw new Exception(checkResult.Errors[0]);

                    Console.WriteLine(task.GroupUrl);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("10 min sleep");
                    Thread.Sleep(10 * 60 * 1000);
                }
            }
        }
    }
}
