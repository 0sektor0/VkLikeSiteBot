using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class PostSource
    {
        [JsonProperty("type", Required = Required.Always)]
        public string Type { get; set; }
    }
}