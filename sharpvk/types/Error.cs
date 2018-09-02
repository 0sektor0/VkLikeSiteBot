using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class Error
    {
        [JsonProperty("error_code", Required = Required.Always)]
        public int ErrorCode { get; set; }

        [JsonProperty("error_msg", Required = Required.Always)]
        public string ErrorMsg { get; set; }

        [JsonProperty("request_params", Required = Required.Always)]
        public List<RequestParam> RequestParams { get; set; }
    }
}