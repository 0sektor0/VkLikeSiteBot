using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public static class Serializer
    {
        public static string ToJson<T>(this Result<T> self) => JsonConvert.SerializeObject(self, SharpVK.Types.Converter.Settings);
    }
}