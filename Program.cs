using System;



namespace VkLikeSiteBot
{
    class Program
    {
        static void Main(string[] args)
        {
            string login = "+79258465151";
            string pass = "YOG_965211-sot";
            SiteAuthentificator siteAuthentificator = new SiteAuthentificator(login, pass);

            siteAuthentificator.Authentificate();

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
