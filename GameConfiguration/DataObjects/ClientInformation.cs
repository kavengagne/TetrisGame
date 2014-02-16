using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace GameConfiguration.DataObjects
{
    public struct ClientInformation
    {
        [JsonProperty]
        public string ServerAddress { get; set; }

        [JsonProperty]
        public string ServerQuery { get; set; }

        [JsonProperty]
        public string WindowName { get; set; }

        [JsonProperty]
        public int WindowWidth { get; set; }

        [JsonProperty]
        public int WindowHeight { get; set; }
    }
}