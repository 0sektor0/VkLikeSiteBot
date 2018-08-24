using System;
using VkLikeSiteBot.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using VkLikeSiteBot.Infrastructure;



namespace VkLikeSiteBot
{
    class Program
    {
        static void Main(string[] args)
        {
            BotSettings settings = BotSettings.GetSettings();
            List<Bot> bots = new List<Bot>();

            for(int i = 0; i < settings.Users.Length; i++)
            {
                SiteUserContext user = settings.Users[i];

                try
                {
                    Bot bot = new Bot(user);
                    bots.Add(bot);

                    Console.WriteLine($"{user.Login} {user.Pass} authorized");
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"{user.Login} {user.Pass} not authorized: {ex.Message}");
                }
            }

            Task[] botsWorkCycles = new Task[bots.Count];
            for(int i = 0; i < bots.Count; i++)
                botsWorkCycles[i] = bots[i].StartAsync();

            Task.WaitAll(botsWorkCycles);
            Console.WriteLine("all bots stoped working");
        }
    }
}
