using System;
using VkLikeSiteBot.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using VkLikeSiteBot.Infrastructure;
using NLog;



namespace VkLikeSiteBot
{
    class Program
    {
        static void Start()
        {
            //Logger logger = LogManager.GetCurrentClassLogger();
            BotSettings settings = BotSettings.GetSettings();
            List<Bot> bots = new List<Bot>();

            for (int i = 0; i < settings.Users.Length; i++)
            {
                SiteUserContext user = settings.Users[i];

                try
                {
                    Bot bot = new Bot(user);
                    bots.Add(bot);

                    //logger.Trace($"{user.Login} {user.Pass} authorized\n");
                    Console.WriteLine($"{user.Login} {user.Pass} authorized\n");
                }
                catch (Exception ex)
                {
                    //logger.Trace($"{user.Login} {user.Pass} not authorized: {ex.Message}");
                    Console.WriteLine($"{user.Login} {user.Pass} not authorized: {ex.Message}");
                }
            }

            Task[] botsWorkCycles = new Task[bots.Count];
            for (int i = 0; i < bots.Count; i++)
                botsWorkCycles[i] = bots[i].StartAsync();

            Task.WaitAll(botsWorkCycles);
            //logger.Error("all bots stoped working");
            Console.WriteLine("all bots stoped working");
        }

        //временный костыль, я пока не понял с чем были связанны периобические выледы при повторной авторизации
        //скорее всего на серверы просто была старая версия, но не стоит исключать, что ошибка где-то тут
        static void Main(string[] args)
        {
            try
            {
                Start();
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{DateTime.UtcNow}\n{ex.Message}");
                Main(null);
            }
        }
    }
}
