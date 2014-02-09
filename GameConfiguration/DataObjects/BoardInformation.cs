using GameConfiguration.Classes;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

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
        [JsonConverter(typeof(RectangleJsonConverter))]
        public Rectangle BlockSize { get; set; }

        [JsonProperty]
        [JsonConverter(typeof(RectangleJsonConverter))]
        public Rectangle PreviewBlockSize { get; set; }

        [JsonProperty]
        public int Speed { get; set; }
    }
}