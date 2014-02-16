using System;
using System.Collections.Generic;
using System.Linq;
using GameClient.Classes.Extensions;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameClient.Classes.Core.Managers
{
    public class TextureManager
    {
        #region Singleton
        private static TextureManager Instance { get; set; }

        public static TextureManager GetInstance()
        {
            return Instance ?? (Instance = new TextureManager());
        }
        #endregion


        #region Fields
        private readonly Dictionary<string, Texture2D> _textures;
        #endregion


        #region Constructor
        private TextureManager()
        {
            _textures = LoadTextures(new ContentManager(TetrisGame.GetInstance().Services, "Content"));
        }
        #endregion


        #region Public Methods
        public Texture2D Get(string textureName)
        {
            Texture2D texture;
            if (!_textures.TryGetValue(textureName, out texture))
            {
                throw new ArgumentException("Parameter textureName is not valid. Value \"{0}\" does not exist.",
                                            textureName);
            }
            return texture;
        }
        #endregion


        #region Internal Implementation
        private static Dictionary<string, Texture2D> LoadTextures(ContentManager contentManager)
        {
            return contentManager.LoadContentFolder<Texture2D>("Graphics");
        }
        #endregion
    }
}