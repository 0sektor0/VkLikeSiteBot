using System;
using sharpvk.Types;
using System.Collections.Generic;



namespace sharpvk
{
    public class ApiClient
    {
        private RequestSender sender;


        public ApiClient(Token t, int maxReqCount)
        {
            sender = new RequestSender(t, maxReqCount);
        }


        public List<Message> SearchMessages(int count, string template)
        {
            Result<ResponseArray<Message>> msgs = sender.Send<ResponseArray<Message>>(new ApiRequest($"messages.search?count={count}&q={template}"));

            if(msgs.IsError())
                throw new VkApiClientException(msgs.Error.ErrorMsg);

            return msgs.Response.Items;
        }


        public List<WallPost> WallGet(int owner_id, int count=1, int offset=0)
        {
            Result<ResponseArray<WallPost>> posts = sender.Send<ResponseArray<WallPost>>(new ApiRequest($"wall.get?extended=0&owner_id={owner_id}&count={count}&offset={offset}"));

            if(posts.IsError())
                throw new VkApiClientException(posts.Error.ErrorMsg);

            return posts.Response.Items;
        }


        public int AddLikeToPost(WallPost post)
        {
            if(!post.Likes.CanLike)
                return 0;

            Result<LikesResponse> resp = sender.Send<LikesResponse>(new ApiRequest($"likes.add?type=post&owner_id={post.OwnerId}&item_id={post.Id}"));

            if(resp.IsError())
                throw new VkApiClientException(resp.Error.ErrorMsg);

            return resp.Response.Likes;
        }


        public int AddLikeToPhoto(AttachmentPhoto photo)
        {
            Result<LikesResponse> resp = sender.Send<LikesResponse>(new ApiRequest($"likes.add?type=photo&owner_id={photo.OwnerId}&item_id={photo.Id}"));

            if(resp.IsError())
                throw new VkApiClientException(resp.Error.ErrorMsg);

            return resp.Response.Likes;
        }


        public void AddLikeToPost(List<WallPost> posts)
        {
            foreach(WallPost post in posts)
                AddLikeToPost(post);
        }


        public bool Repost(WallPost post)
        {
            Result<RepostResponse> resp = sender.Send<RepostResponse>(new ApiRequest($"wall.repost?object=wall{post.OwnerId}_{post.Id}"));

            if(resp.IsError())
                throw new VkApiClientException(resp.Error.ErrorMsg);

            return resp.Response.Success;
        }


        public int CopyMessage(Message msg, int uid)
        {
            Result<int> resp = sender.Send<int>(ConvertMessageToReq(msg, uid));

            if(resp.IsError())
                throw new VkApiClientException(resp.Error.ErrorMsg);

            return resp.Response;
        }


        public List<Profile> GetGroupMembers(int group_id, int count, int offset)
        {
            Result<ResponseArray<Profile>> profiles = sender.Send<ResponseArray<Profile>>(new ApiRequest($"groups.getMembers?group_id={group_id}&count={count}&offset={offset}&fields=contacts,online,online,online_mobile,sex"));

            if(profiles.IsError())
                throw new VkApiClientException(profiles.Error.ErrorMsg);

            return profiles.Response.Items;
        }


        private ApiRequest ConvertMessageToReq(Message msg, int uid)
        {
            Dictionary<string, string> prms = new Dictionary<string, string>();
            prms["user_id"] = uid.ToString();
            
            if(msg.Text == "" && msg.Attachments.Count == 0)
                throw new VkApiClientException("text ot attachments must be declared in message");

            if(msg.Text != "")
                prms["message"] = msg.Text;

            if(msg.Attachments.Count != 0)
            {
                string att = "";
                foreach(Attachment a in msg.Attachments)
                    att += $",{a.ToString()}";
                prms["attachment"] = att.Substring(1, att.Length-1); 
            }

            return new ApiRequest("messages.send", prms);
        }


        public int JoinGroup(int group_id)
        {
            Result<int> resp = sender.Send<int>(new ApiRequest($"groups.join?group_id={group_id}"));

            if(resp.IsError())
                throw new VkApiClientException(resp.Error.ErrorMsg);

            return resp.Response;
        }
    }
}