using System.Drawing;
using Newtonsoft.Json;
using Color = Microsoft.Xna.Framework.Color;

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
        public Size BlockSize { get; set; }

        [JsonProperty]
        public int Speed { get; set; }
    }
}