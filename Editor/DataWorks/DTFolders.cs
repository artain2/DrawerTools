using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace DrawerTools
{
    public static class DTFolders
    {
        private const string ASSETS = "Assets";
        private const string RESOURCES = "Resources";

        public static bool HasFolder(string path, SavePath pathType = SavePath.Custom)
        {
            path = ValidatePath(path, pathType);
            return Directory.Exists(path);
        }

        public static bool HasFolder(string path, string name) =>
            HasFolder($"{path}/{name}");

        public static void CreateFolder(string path)
        {
            string name = Path.GetFileName(path.TrimEnd(Path.DirectorySeparatorChar));
            path = Path.GetDirectoryName(path);
            CreateFolder(path, name);
        }

        public static string CreateFolder(string path, string name) =>
            AssetDatabase.CreateFolder(path, name);

        public static bool SoftCreateFolder(string path, SavePath pathType = SavePath.Custom)
        {
            path = ValidatePath(path, pathType);
            string name = Path.GetFileName(path.TrimEnd(Path.DirectorySeparatorChar));
            path = Path.GetDirectoryName(path);
            return SoftCreateFolder(path, name);
        }

        public static bool SoftCreateFolder(string path, string name, SavePath pathType = SavePath.Custom)
        {
            path = ValidatePath(path, pathType);
            if (HasFolder(path, name))
            {
                return false;
            }
            AssetDatabase.CreateFolder(path, name);
            return true;
        }

        public static void DeleteFolder(string path, SavePath pathType = SavePath.Custom)
        {
            path = ValidatePath(path, pathType);
            FileUtil.DeleteFileOrDirectory(path);
            AssetDatabase.Refresh();
        }

        public static string ValidatePath(string path, SavePath pathType = SavePath.Custom)
        {
            switch (pathType)
            {
                case SavePath.Assets:
                    if (path.Substring(0, ASSETS.Length) == ASSETS)
                    {
                        path = path.Substring(ASSETS.Length);
                    }
                    return Application.dataPath + path;
                case SavePath.Resources:
                    if (path.Length >= ASSETS.Length && path.Substring(0, ASSETS.Length) == ASSETS)
                    {
                        path = path.Substring(ASSETS.Length).TrimStart('/');
                    }
                    if (path.Length >= RESOURCES.Length && path.Substring(0, RESOURCES.Length) == RESOURCES)
                    {
                        path = path.Substring(RESOURCES.Length).TrimStart('/');
                    }
                    return $"{Application.dataPath}/{RESOURCES}/{path}";
                case SavePath.Saves:
                    return $"{Application.persistentDataPath}/{path}";
                default:
                    return path;
            }
        }

        public static List<string> GetAllFilePathsInFolder(string assetsFolder)
        {
            var paths = Directory.GetFiles(assetsFolder);
            return paths.Where(x => !x.Contains(".meta")).ToList();
        }
    }
}