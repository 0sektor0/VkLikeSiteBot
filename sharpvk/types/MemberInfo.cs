using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class MemberInfo
    {
        [JsonProperty("member_id", Required = Required.Always)]
        public int MemberId { get; set; }

        [JsonProperty("join_date", Required = Required.Always)]
        public int JoinDate { get; set; }

        [JsonProperty("invited_by", Required = Required.Always)]
        public int InvitedBy { get; set; }

        [JsonProperty("is_admin", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public bool? IsAdmin { get; set; }
    }
}