using System;



namespace VkLikeSiteBot
{
    class Program
    {
        static void Main(string[] args)
        {
            string login = "+79104876472";
            string pass = "Ra_965211-de";
            SiteAuthentificator siteAuthentificator = new SiteAuthentificator();

            siteAuthentificator.Authentificate(login, pass);

            string uid = "472951660";
            string token = "202a38e8f5414603373942f783acf7ec";

            SiteParser parser = new SiteParser();
            SiteClient client = new SiteClient(token, uid, parser);
            var res = client.ReciveTask();
            res = client.ReciveTask();

            Console.WriteLine("Hello World!");
        }
    }
}
