using System;
using VkLikeSiteBot.Models;
using VkLikeSiteBot.Interfaces;
using System.Text.RegularExpressions;
using System.Collections.Generic;



namespace VkLikeSiteBot
{
    public class SiteParser
    {
        public BotJoinTask ParseJoinTask(string html)
        {
            Regex regex = new Regex(@"doCompany\((.+),(.+),(.+), (.+)\)");
            Match match = regex.Match(html);

            if (!match.Success)
                return null;

            return new BotJoinTask
            {
                taskId = match.Groups[1].Value,
                groupId = match.Groups[2].Value,
                groupUrl = match.Groups[3].Value,
                api = match.Groups[4].Value
            };
        }


        public BotLikeTask ParseLikeTask(string html)
        {
            Regex regex = new Regex(@"do_like\((.+),(.+),(.+),'(.+)','(.+)','(.+)', (.+)\)");
            Match match = regex.Match(html);

            if (!match.Success)
                return null;

            return new BotLikeTask
            {
                taskId = match.Groups[1].Value,
                postId = Convert.ToInt32(match.Groups[2].Value),
                ownerId = Convert.ToInt32(match.Groups[3].Value),
                type = match.Groups[4].Value,
                postUrl = match.Groups[5].Value,
                repost = match.Groups[6].Value,
                api = match.Groups[7].Value
            };
        }
    }
}