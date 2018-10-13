using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class PushSettings
    {
        [JsonProperty("no_sound", Required = Required.Always)]
        public bool NoSound { get; set; }

        [JsonProperty("disabled_until", Required = Required.Always)]
        public int DisabledUntil { get; set; }

        [JsonProperty("disabled_forever", Required = Required.Always)]
        public bool DisabledForever { get; set; }
    }
}