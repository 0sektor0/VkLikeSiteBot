using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class AttachmentPhoto : ILikeableItem
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

        [JsonProperty("album_id")]
        public int AlbumId { get; set; }

        [JsonProperty("sizes")]
        public List<Size> Sizes { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }


        public override string ToString()
        {
            string str = $"photo{OwnerId}_{Id}";

            if (AccessKey != "")
                return str + $"_{AccessKey}";

            return str;
        }
    }
}