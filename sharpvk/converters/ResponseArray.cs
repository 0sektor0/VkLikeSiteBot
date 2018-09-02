using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    //TODO delete Count
    public class ResponseArray<T>
    {
        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("items", Required = Required.Always)]
        public List<T> Items { get; set; }
    }
}