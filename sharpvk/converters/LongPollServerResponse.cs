using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class LongPollServerResponse
    {
        [JsonProperty("key", Required = Required.Always)]
        public string Key { get; set; }

        [JsonProperty("server", Required = Required.Always)]
        public string Server { get; set; }

        [JsonProperty("ts", Required = Required.Always)]
        public int Ts { get; set; }
    }
}