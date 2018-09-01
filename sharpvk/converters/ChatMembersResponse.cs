using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class ChatMembersResponse
    {
        [JsonProperty("items", Required = Required.Always)]
        public List<MemberInfo> MemberInfo { get; set; }

        [JsonProperty("count", Required = Required.Always)]
        public int Count { get; set; }

        [JsonProperty("profiles", Required = Required.Always)]
        public List<Profile> Profiles { get; set; }
    }
}