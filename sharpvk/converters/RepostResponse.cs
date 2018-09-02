using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class RepostResponse
    {
        [JsonProperty("success", Required = Required.Always)]
        public bool Success { get; set; }

        [JsonProperty("post_id", Required = Required.Always)]
        public int PostId { get; set; }

        [JsonProperty("reposts_count", Required = Required.Always)]
        public int RepostsCount { get; set; }

        [JsonProperty("likes_count", Required = Required.Always)]
        public int LikesCount { get; set; }
    }

}