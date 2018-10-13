using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class EmptyAttach
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
    }
}