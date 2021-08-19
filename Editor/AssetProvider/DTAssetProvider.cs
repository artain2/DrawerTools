using UnityEngine;
using UnityEditor;

namespace DrawerTools
{
    public class DTAssetProvider<T> where T : ScriptableObject, new()
    {
        public DTAssetProvider(string path)
        {
            Path = path;
        }

        public string Path { get; protected set; }
        public T Config => (T)AssetDatabase.LoadAssetAtPath(Path, typeof(T));
        public T GetOrCreateConfig()
        {
            var asset = Config;
            if (asset == null)
            {
                asset = new T();
                AssetDatabase.CreateAsset(asset, Path);
            }
            return asset;
        }
        public bool HasAsset => Config != null;
        public void SaveAsset() => EditorUtility.SetDirty(Config);
    }
}