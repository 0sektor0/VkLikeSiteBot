using System;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace VkLikeSiteBot
{

    public class BotSettings
    {
        public static string path = Directory.GetCurrentDirectory()+"/data/botconfig.json";
        private static readonly BotSettings instanse = LoadConfigs(path);

        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("pass")]
        public string Pass { get; set; }

        [JsonProperty("uid")]
        public string Uid { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("host")]
        public string Host { get; set; }

        [JsonProperty("check_delay")]
        public int CheckDelay { get; set; } = 10;

        [JsonProperty("recieve_delay")]
        public int RecieveDelay { get; set; } = 10;


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
