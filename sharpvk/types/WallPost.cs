using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class WallPost : ILikeableItem
    {
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("owner_id", Required = Required.Always)]
        public int OwnerId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("date")]
        public int Date { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }

        [JsonProperty("from_id", Required = Required.Always)]
        public int FromId { get; set; }

        [JsonProperty("marked_as_ads")]
        public int MarkedAsAds { get; set; }

        [JsonProperty("post_type")]
        public string PostType { get; set; }

        [JsonProperty("text", Required = Required.Always)]
        public string Text { get; set; }

        [JsonProperty("is_pinned")]
        public int IsPinned { get; set; }

        [JsonProperty("attachments")]
        public List<Attachment> Attachments { get; set; }

        [JsonProperty("post_source")]
        public PostSource PostSource { get; set; }

        [JsonProperty("comments")]
        public Comments Comments { get; set; }

        [JsonProperty("likes")]
        public Likes Likes { get; set; }

        [JsonProperty("reposts", Required = Required.Always)]
        public Reposts Reposts { get; set; }

        [JsonProperty("views")]
        public Views Views { get; set; }
    }
}