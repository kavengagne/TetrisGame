using Newtonsoft.Json;
using Microsoft.Xna.Framework;

namespace GameConfiguration.DataObjects
{
    public struct BoardInformation
    {
        [JsonProperty]
        public int Rows { get; set; }

        [JsonProperty]
        public int Columns { get; set; }

        [JsonProperty]
        public Color BackgroundColor { get; set; }

        [JsonProperty]
        public Rectangle BlockSize { get; set; }

        [JsonProperty]
        public int Speed { get; set; }
    }
}