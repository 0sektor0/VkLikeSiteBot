using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class LikesResponse
    {
        [JsonProperty("likes", Required = Required.Always)]
        public int Likes { get; set; }
    }

}