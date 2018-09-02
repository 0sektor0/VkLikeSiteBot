using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class ChatPhoto
    {
        [JsonProperty("photo_50", Required = Required.Always)]
        public string Photo50 { get; set; }

        [JsonProperty("photo_100", Required = Required.Always)]
        public string Photo100 { get; set; }

        [JsonProperty("photo_200", Required = Required.Always)]
        public string Photo200 { get; set; }
    }
}