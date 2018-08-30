using VkLikeSiteBot.Models;
using System.Collections.Generic;
using VkLikeSiteBot.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Http;
using sharpvk.Types;
using sharpvk;
using System;



namespace VkLikeSiteBot.Infrastructure
{
    public class Bot
    {
        private SiteUserContext _siteUser;
        private SiteClient _siteClient;
        private ApiClient _vkClient;


        public Bot(SiteUserContext siteUser)
        {
            _siteUser = siteUser;
            _siteClient = new SiteClient(siteUser);

            Token t = new Token(siteUser.Login, siteUser.Pass, 274556);
            _vkClient = new ApiClient(t, 3);
        }


        public void Start()
        {
            while (true)
            {
                List<IBotTask> tasks = _siteClient.ReciveTask();

                if (tasks.Count == 0)
                {
                    Thread.Sleep(_siteUser.RecieveDelay * 60 * 1000);
                    continue;
                }

                Console.WriteLine($"\nbot {_siteUser.Login} tasks count: {tasks.Count}");
                foreach (IBotTask task in tasks)
                {
                    string report = $"\nbot: {_siteUser.Login}\n{DateTime.UtcNow}\ntask\n{task.ToString()}";

                    try
                    {
                        switch(task.Type)
                        {
                            case BotTaskType.JoinGroup :
                                HandleJoinTask(task as BotJoinTask);
                                break;

                            case BotTaskType.LikePhoto :
                                HandlePhotoLikeTask(task as BotLikeTask);
                                break;

                            case BotTaskType.LikePost :
                                HandlePostLikeTask(task as BotLikeTask);
                                break;

                            default :
                                throw new Exception("undefined task type");
                        }

                        Thread.Sleep(_siteUser.CheckDelay * 1000);

                        var checkResult = _siteClient.CheckTask(task);
                        if (!checkResult.Success)
                        {
                            report += "\nstatus: Task failed";
                            foreach(string error in checkResult.Errors)
                                report += $"\nerror: {error}";
                        }
                        else
                            report += "\nstatus: Task completed";

                        Console.WriteLine(report);                            
                    }
                    catch (VkApiClientException ex)
                    {
                        report += $"\nVk client exception: {ex.Message}";
                        Console.WriteLine(report);

                        Thread.Sleep(_siteUser.RecieveDelay * 60 * 1000);
                    }
                    catch (Exception ex)
                    {
                        _siteClient.RefuseTask(task);

                        report += $"\nexception: {ex.Message}";
                        Console.WriteLine(report);

                        Thread.Sleep(_siteUser.RecieveDelay * 60 * 1000);
                    }
                }
            }
        }


        public Task StartAsync()
        {
            return Task.Run(() => Start());
        }



        private void HandleJoinTask(BotJoinTask task)
        {
            Group group = _vkClient.GetGroup(task.groupId);

            if(group.IsClosed == 0)
                _vkClient.JoinGroup(Convert.ToInt32(task.groupId));
            else
                throw new Exception($"{task.groupUrl} is closed group");
        }


        private void HandlePostLikeTask(BotLikeTask task)
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

                if (_vkClient.AddLikeToPost(post) == 0)
                    throw new Exception($"cannot like post {task.postUrl}");

                if (task.repost == "1")
                    if (!_vkClient.Repost(post))
                        throw new Exception($"cannot repost post {task.postUrl}");
        }


        private void HandlePhotoLikeTask(BotLikeTask task)
        {
                AttachmentPhoto photo = new AttachmentPhoto
                {
                    OwnerId = task.ownerId,
                    Id = task.postId
                };

                if (_vkClient.AddLikeToPhoto(photo) == 0)
                    throw new Exception($"cannot like photo {task.postUrl}");
        }
    }
}