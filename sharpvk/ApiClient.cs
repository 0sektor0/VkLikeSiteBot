using System;
using SharpVK.Types;
using System.Collections.Generic;



namespace SharpVK
{
    public class ApiClient
    {
        private RequestSender _sender;


        public ApiClient(Token t, int maxReqCount)
        {
            _sender = new RequestSender(t, maxReqCount);
        }


        public List<Message> SearchMessages(int count, string template)
        {
            Result<ResponseArray<Message>> msgs = _sender.Send<ResponseArray<Message>>(new ApiRequest($"messages.search?count={count}&q={template}"));

            if(msgs.IsError())
                throw new VkApiClientException(msgs.Error.ErrorMsg);

            return msgs.Response.Items;
        }


        public List<WallPost> WallGet(int owner_id, int count=1, int offset=0)
        {
            Result<ResponseArray<WallPost>> posts = _sender.Send<ResponseArray<WallPost>>(new ApiRequest($"wall.get?extended=0&owner_id={owner_id}&count={count}&offset={offset}"));

            if(posts.IsError())
                throw new VkApiClientException(posts.Error.ErrorMsg);

            return posts.Response.Items;
        }


        public int AddLikeToItem(ILikeableItem item, string type)
        {
            Result<LikesResponse> resp = _sender.Send<LikesResponse>(new ApiRequest($"likes.add?type={type}&owner_id={item.OwnerId}&item_id={item.Id}"));

            if(resp.IsError())
                throw new VkApiClientException(resp.Error.ErrorMsg);

            return resp.Response.Likes;
        }


        public bool Repost(WallPost post)
        {
            Result<RepostResponse> resp = _sender.Send<RepostResponse>(new ApiRequest($"wall.repost?object=wall{post.OwnerId}_{post.Id}"));

            if(resp.IsError())
                throw new VkApiClientException(resp.Error.ErrorMsg);

            return resp.Response.Success;
        }


        public int CopyMessage(Message msg, int uid)
        {
            Result<int> resp = _sender.Send<int>(ConvertMessageToReq(msg, uid));

            if(resp.IsError())
                throw new VkApiClientException(resp.Error.ErrorMsg);

            return resp.Response;
        }


        public List<Profile> GetGroupMembers(int group_id, int count, int offset)
        {
            Result<ResponseArray<Profile>> profiles = _sender.Send<ResponseArray<Profile>>(new ApiRequest($"groups.getMembers?group_id={group_id}&count={count}&offset={offset}&fields=contacts,online,online,online_mobile,sex"));

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


        public Group GetGroup(string name)
        {
            Result<Group[]> resp = _sender.Send<Group[]>(new ApiRequest($"groups.getById?group_ids={name}"));

            if(resp.IsError())
                throw new VkApiClientException(resp.Error.ErrorMsg);

            if(resp.Response.Length == 0)
                return null;
            else
                return resp.Response[0];
        }


        public Group GetGroup(int id)
        {
            return GetGroup(id.ToString());
        }


        public int JoinGroup(int group_id)
        {
            Result<int> resp = _sender.Send<int>(new ApiRequest($"groups.join?group_id={group_id}"));

            if(resp.IsError())
                throw new VkApiClientException(resp.Error.ErrorMsg);

            return resp.Response;
        }
    }
}