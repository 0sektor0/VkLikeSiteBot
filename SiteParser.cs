using VkLikeSiteBot.Models;
using VkLikeSiteBot.Interfaces;
using System.Text.RegularExpressions;



namespace VkLikeSiteBot
{
    public class SiteParser : ISiteParser
    {
        public Result<BotTask> Parse(string html)
        {
            Regex regex = new Regex(@"doCompany\((.+),(.+),(.+), (.+)\)");
            Match match = regex.Match(html);

            if (!match.Success)
                return new Result<BotTask>("empty task");

            BotTask task = new BotTask
            {
                TaskId = match.Groups[1].Value,
                GroupId = match.Groups[2].Value,
                GroupUrl = match.Groups[3].Value,
                Api = match.Groups[4].Value
            };

            return new Result<BotTask>(task);
        }
    }
}