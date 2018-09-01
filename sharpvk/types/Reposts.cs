using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class Reposts
    {
        [JsonProperty("count", Required = Required.Always)]
        public int Count { get; set; }

        [JsonProperty("user_reposted", Required = Required.Always)]
        public bool UserReposted { get; set; }
    }
}