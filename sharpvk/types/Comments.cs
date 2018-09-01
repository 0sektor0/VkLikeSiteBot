using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class Comments
    {
        [JsonProperty("count", Required = Required.Always)]
        public int Count { get; set; }

        [JsonProperty("groups_can_post")]
        public bool GroupsCanPost { get; set; }

        [JsonProperty("can_post", Required = Required.Always)]
        public int CanPost { get; set; }
    }
}