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
        private static BotSettings _instance;
        private static string _path = Directory.GetCurrentDirectory() + "/data/botconfig.json";

        
        public static BotSettings Instance
        {
            get 
            {
                if(_instance == null)
                    _instance = LoadConfigs(_path);

                return _instance;
            }
        }

        [JsonProperty("users")]
        public SiteUserContext[] Users { get; set; }

        [JsonProperty("debug")]
        public bool Debug { get; set; }


        public static void SetPath(string path) => BotSettings._path = path;

        private static BotSettings FromJson(string json) => JsonConvert.DeserializeObject<BotSettings>(json, Converter.Settings);

        private static BotSettings LoadConfigs(string path)
        {
            BotSettings s;

            if (!File.Exists(path))
                throw new FileNotFoundException($"{path} not found");

            using (StreamReader reader = new StreamReader(path))
                s = FromJson(reader.ReadToEnd());

            return s;
        }

        public void ReloadConfig()
        {
            LoadConfigs(_path);
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
