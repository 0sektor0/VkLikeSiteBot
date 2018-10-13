using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    //TODO дообъявить все типы и вынести аттачи в отдельное пространство имен    
    public class Attachment
    {
        [JsonProperty("type", Required = Required.Always)]
        public TypeEnum Type { get; set; }

        [JsonProperty("audio", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Audio Audio { get; set; }

        [JsonProperty("doc", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Document Doc { get; set; }

        [JsonProperty("photo", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public AttachmentPhoto Photo { get; set; }

        [JsonProperty("video", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public Video Video { get; set; }

        [JsonProperty("empty", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public EmptyAttach EmptyAttach { get; set; }


        public override string ToString()
        {
            switch (Type)
            {
                case TypeEnum.Audio:
                    return Audio.ToString();
                case TypeEnum.Doc:
                    return Doc.ToString();
                case TypeEnum.Photo:
                    return Photo.ToString();
                case TypeEnum.Video:
                    return Video.ToString();
                default:
                    throw new Exception($"undefined type {Type}");
            }
        }
    }
}