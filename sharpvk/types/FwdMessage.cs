using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class FwdMessage
    {
        [JsonProperty("date", Required = Required.Always)]
        public int Date { get; set; }

        [JsonProperty("from_id", Required = Required.Always)]
        public int FromId { get; set; }

        [JsonProperty("text", Required = Required.Always)]
        public string Text { get; set; }

        [JsonProperty("attachments", Required = Required.Always)]
        public List<Attachment> Attachments { get; set; }

        [JsonProperty("update_time", Required = Required.Always)]
        public int UpdateTime { get; set; }
    }
}