using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class Likes
    {
        [JsonProperty("count", Required = Required.Always)]
        public int Count { get; set; }

        [JsonProperty("user_likes", Required = Required.Always)]
        public int UserLikes { get; set; }

        [JsonProperty("can_like", Required = Required.Always)]
        public bool CanLike { get; set; }

        [JsonProperty("can_publish", Required = Required.Always)]
        public bool CanPublish { get; set; }
    }
}