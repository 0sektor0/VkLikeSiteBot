using System.Net.Http;
using Newtonsoft.Json;



namespace VkLikeSiteBot.Models
{
    public class SiteUserContext
    {
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
        public double CheckDelay { get; set; } = 10;

        [JsonProperty("recieve_delay")]
        public double RecieveDelay { get; set; } = 10;
    }
}
