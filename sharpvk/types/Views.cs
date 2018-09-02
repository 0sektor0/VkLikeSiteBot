using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class Views
    {
        [JsonProperty("count", Required = Required.Always)]
        public int Count { get; set; }
    }
}
