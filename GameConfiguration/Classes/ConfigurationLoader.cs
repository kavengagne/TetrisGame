using System.IO;
using System.Net;
using GameConfiguration.DataObjects;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace GameConfiguration.Classes
{
    public static class ConfigurationLoader
    {
        #region Public Methods
        public static ClientInformation GetClientConfiguration()
        {
            // TODO: KG - Add error handling
            var reader = new StreamReader(TitleContainer.OpenStream("Content/config.json"));
            return JsonConvert.DeserializeObject<ClientInformation>(reader.ReadToEnd());
        }

        public static Configuration GetServerConfiguration(ClientInformation clientInformation)
        {
            var client = new WebClient
            {
                BaseAddress = clientInformation.ServerAddress
            };
            var configString = client.DownloadString(clientInformation.ServerQuery);
            return JsonConvert.DeserializeObject<Configuration>(configString);
        }
        #endregion
    }
}