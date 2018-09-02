using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class Conversation
    {
        [JsonProperty("peer", Required = Required.Always)]
        public Peer Peer { get; set; }

        [JsonProperty("in_read", Required = Required.Always)]
        public int InRead { get; set; }

        [JsonProperty("out_read", Required = Required.Always)]
        public int OutRead { get; set; }

        [JsonProperty("last_message_id", Required = Required.Always)]
        public int LastMessageId { get; set; }

        [JsonProperty("can_write", Required = Required.Always)]
        public CanWrite CanWrite { get; set; }

        [JsonProperty("chat_settings")]
        public ChatSettings ChatSettings { get; set; }

        [JsonProperty("push_settings")]
        public PushSettings PushSettings { get; set; }
    }
}