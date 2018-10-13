using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class Message
    {
        [JsonProperty("date", Required = Required.Always)]
        public int Date { get; set; }

        [JsonProperty("from_id", Required = Required.Always)]
        public int FromId { get; set; }

        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("out", Required = Required.Always)]
        public int Out { get; set; }

        [JsonProperty("peer_id", Required = Required.Always)]
        public int PeerId { get; set; }

        [JsonProperty("text", Required = Required.Always)]
        public string Text { get; set; }

        [JsonProperty("conversation_message_id", Required = Required.Always)]
        public int ConversationMessageId { get; set; }

        [JsonProperty("fwd_messages")]
        public List<FwdMessage> FwdMessages { get; set; }

        [JsonProperty("important", Required = Required.Always)]
        public bool Important { get; set; }

        [JsonProperty("random_id", Required = Required.Always)]
        public int RandomId { get; set; }

        [JsonProperty("attachments", Required = Required.Always)]
        public List<Attachment> Attachments { get; set; }

        [JsonProperty("is_hidden", Required = Required.Always)]
        public bool IsHidden { get; set; }
    }
}