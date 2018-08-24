using System;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
using VkLikeSiteBot.Models;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace VkLikeSiteBot.Infrastructure
{

    public class BotSettings
    {
        public static string path = Directory.GetCurrentDirectory()+"/data/botconfig.json";

        private static readonly BotSettings instanse = LoadConfigs(path);

        [JsonProperty("users")]
        public SiteUserContext[] Users { get; set; }


        public static void SetPath(string path) => BotSettings.path = path;


        public static BotSettings GetSettings() => instanse;


        private static BotSettings FromJson(string json) => JsonConvert.DeserializeObject<BotSettings>(json, Converter.Settings);


        private static BotSettings LoadConfigs(string path)
        {
            BotSettings s;

            if(!File.Exists(path))
                throw new FileNotFoundException($"{path} not found");

            using(StreamReader reader = new StreamReader(path))
                s = FromJson(reader.ReadToEnd());

            return s;
        }
    }



    internal static class Serialize
    {
        public static string ToJson(this BotSettings self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }



    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters = {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
