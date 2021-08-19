using UnityEngine;
using UnityEditor;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace DrawerTools
{
    public static class DTSerrializer
    {
        public static string AssetsPath(UnityEngine.Object obj) => AssetDatabase.GetAssetPath(obj);

        public static void Save<T>(T data, string path, SavePath pathType = SavePath.Custom) where T : new()
        {
            path = DTFolders.ValidatePath(path, pathType);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
            bf.Serialize(fs, data);
            fs.Close();
            AssetDatabase.Refresh();
        }

        public static T Load<T>(string path, SavePath pathType = SavePath.Custom) where T : new()
        {
            path = DTFolders.ValidatePath(path, pathType);
            if (!Exists(path))
            {
                return default;
            }

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            var data = (T)bf.Deserialize(fs);
            fs.Close();
            return data;
        }

        public static bool Exists(string path) => File.Exists(path);

        public static void Remove(string path)
        {
            if (!Exists(path))
            {
                return;
            }
            File.Delete(path);
        }
    }
}