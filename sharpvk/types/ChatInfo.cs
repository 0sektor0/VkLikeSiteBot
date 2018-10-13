using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class ChatInfo
    {
        [JsonProperty("conversation", Required = Required.Always)]
        public Conversation Conversation { get; set; }

        [JsonProperty("last_message", Required = Required.Always)]
        public Message LastMessage { get; set; }
    }
}