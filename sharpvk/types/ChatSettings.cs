using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class ChatSettings
    {
        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        [JsonProperty("members_count")]
        public int MembersCount { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("photo")]
        public ChatPhoto ChatPhoto { get; set; }

        [JsonProperty("active_ids")]
        public List<long> ActiveIds { get; set; }
    }
}