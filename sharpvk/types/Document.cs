using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class Document : ILikeableItem
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

        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        [JsonProperty("size", Required = Required.Always)]
        public int Size { get; set; }

        [JsonProperty("ext", Required = Required.Always)]
        public string Ext { get; set; }

        [JsonProperty("type", Required = Required.Always)]
        public int Type { get; set; }


        public override string ToString()
        {
            string str = $"doc{OwnerId}_{Id}";

            if (AccessKey != "")
                return str + $"_{AccessKey}";

            return str;
        }
    }
}