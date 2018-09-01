using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public enum TypeEnum { Audio, Doc, Photo, Video, EmptyAttach };



    internal class TypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);


        //TODO: remove EmptyAttach mockup
        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "audio":
                    return TypeEnum.Audio;
                case "doc":
                    return TypeEnum.Doc;
                case "photo":
                    return TypeEnum.Photo;
                case "video":
                    return TypeEnum.Video;
                case "link":
                    return TypeEnum.EmptyAttach;
                case "wall":
                    return TypeEnum.EmptyAttach;
                case "sticker":
                    return TypeEnum.EmptyAttach;
                case "gift":
                    return TypeEnum.EmptyAttach;
                case "market":
                    return TypeEnum.EmptyAttach;
                case "wall_reply":
                    return TypeEnum.EmptyAttach;
                default:
                    return TypeEnum.EmptyAttach;
            }
            throw new Exception($"Cannot unmarshal type TypeEnum: {value}");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (TypeEnum)untypedValue;
            switch (value)
            {
                case TypeEnum.Audio:
                    serializer.Serialize(writer, "audio");
                    return;
                case TypeEnum.Doc:
                    serializer.Serialize(writer, "doc");
                    return;
                case TypeEnum.Photo:
                    serializer.Serialize(writer, "photo");
                    return;
                case TypeEnum.Video:
                    serializer.Serialize(writer, "video");
                    return;
            }
            throw new Exception("Cannot marshal type TypeEnum");
        }

        public static readonly TypeEnumConverter Singleton = new TypeEnumConverter();
    }

}