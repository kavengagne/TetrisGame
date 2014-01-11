using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace GameConfiguration.DataObjects
{
    public struct GameInformation
    {
        [JsonProperty]
        public Color BackgroundColor { get; set; }
    }
}