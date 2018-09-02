using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class Result<T>
    {
        [JsonProperty("error", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Error Error { get; set; }

        [JsonProperty("response", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public T Response { get; set; }

        public static Result<T> FromJson(string json) => JsonConvert.DeserializeObject<Result<T>>(json, SharpVK.Types.Converter.Settings);

        public bool IsError()
        {
            if (IsEmpty())
                throw new Exception("Responses is empty");
            else if (Error != null)
                return true;

            return false;
        }

        public bool IsEmpty()
        {
            return Error == null && Response == null;
        }
    }
}