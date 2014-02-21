using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework.Content;

namespace GameClient.Classes.Extensions
{
    public static class ContentManagerExtensions
    {
        #region Public Methods
        public static Dictionary<string, T> LoadContentFolder<T>(this ContentManager contentManager, string folderName)
        {
            var list = new Dictionary<string, T>();
            var filesList = Directory.EnumerateFiles(Directory.GetCurrentDirectory() + @"\Content\" + folderName,
                                                     "*.xnb",
                                                     SearchOption.AllDirectories);
            foreach (var fileName in filesList)
            {
                var pathParts = fileName.Split(new[] { '\\', '/' });
                var pathList = pathParts.SkipWhile(p => !p.Equals("Content")).Skip(1)
                                        .Select(Path.GetFileNameWithoutExtension).ToList();
                var assetName = Path.GetFileNameWithoutExtension(pathList.Last());
                var assetPath = String.Join("/", pathList);
                var assetData = contentManager.Load<T>(assetPath);
                if (assetName != null)
                {
                    list.Add(assetName, assetData);
                }
            }
            return list;
        }
        #endregion
    }
}
