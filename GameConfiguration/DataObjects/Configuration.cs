using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace GameConfiguration.DataObjects
{
    public class Configuration
    {
        [JsonProperty]
        public BoardInformation Board { get; set; }

        [JsonProperty]
        public PieceInformation[] Pieces { get; set; }

        [JsonProperty]
        public Color[] PiecesColors { get; set; }
    }
}