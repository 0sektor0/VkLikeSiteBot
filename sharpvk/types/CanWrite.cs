using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class CanWrite
    {
        [JsonProperty("allowed", Required = Required.Always)]
        public bool Allowed { get; set; }
    }
}