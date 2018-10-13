using System;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;
using Newtonsoft.Json.Converters;



namespace SharpVK.Types
{
    public class Audio : ILikeableItem
    {
        [JsonProperty("id", Required = Required.Always)]
        public int Id { get; set; }

        [JsonProperty("owner_id", Required = Required.Always)]
        public int OwnerId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("date")]
        public int Date { get; set; }

        [JsonProperty("access_key")]
        public string AccessKey { get; set; }

        [JsonProperty("artist", Required = Required.Always)]
        public string Artist { get; set; }

        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; set; }

        [JsonProperty("duration", Required = Required.Always)]
        public int Duration { get; set; }

        [JsonProperty("lyrics_id", Required = Required.DisallowNull, NullValueHandling = NullValueHandling.Ignore)]
        public long? LyricsId { get; set; }

        [JsonProperty("genre_id")]
        public int GenreId { get; set; }

        [JsonProperty("is_hq", Required = Required.Always)]
        public bool IsHq { get; set; }


        public override string ToString()
        {
            string str = $"audio{OwnerId}_{Id}";

            if (AccessKey != "")
                return str + $"_{AccessKey}";

            return str;
        }
    }
}