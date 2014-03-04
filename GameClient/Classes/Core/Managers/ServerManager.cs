using System;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using GameClient.Classes.Core.Settings;
using GameModel.Models;
using Newtonsoft.Json;


namespace GameClient.Classes.Core.Managers
{
    public class ServerManager : IDisposable
    {
        #region Singleton Pattern
        private static ServerManager Instance { get; set; }

        public static ServerManager GetInstance()
        {
            return Instance ?? (Instance = new ServerManager());
        }
        #endregion


        #region Fields
        private HttpClient _client;
        #endregion


        #region Constructor
        private ServerManager()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(Defaults.Server.Address)
            };
            //request.Content = new StringContent("", Encoding.UTF8, "application/json");
            //request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }
        #endregion


        #region Public Methods
        public User GetUser()
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, Defaults.Server.Query.Users))
            {
                request.Headers.Authorization = GetUserCredentials();
                var user = _client.SendAsync(request).ContinueWith(response =>
                {
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var userString = response.Result.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<User>(userString);
                    }
                    return null;
                }).Result;
                return user;
            }
        }

        public bool CreateUser()
        {
            return true;
        }

        private static AuthenticationHeaderValue GetUserCredentials()
        {
            var passBytes = Encoding.ASCII.GetBytes("Kaven:password");
            var base64String = Convert.ToBase64String(passBytes);
            return new AuthenticationHeaderValue("Basic", base64String);
        }
        #endregion


        #region Implement IDisposable Interface
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ServerManager()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_client != null)
                {
                    _client.Dispose();
                    _client = null;
                }
            }
        }
        #endregion
    }
}
