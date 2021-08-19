using UnityEngine;
using UnityEditor;

namespace DrawerTools
{
    public class DTMuliAssetProvider<T> : DTMultiAssetProvider where T : ScriptableObject, new()
    {
        public DTMuliAssetProvider(string pathFormat) : base(pathFormat) { }

        public T GetConfig(params string[] nameParams) => (T)AssetDatabase.LoadAssetAtPath(GetPath(nameParams), typeof(T));

        public bool HasAsset(params string[] nameParams) => GetConfig(nameParams) != null;

        public T GetOrCreateConfig(params string[] nameParams)
        {
            var asset = GetConfig(nameParams);
            if (asset == null)
            {
                asset = new T();
                AssetDatabase.CreateAsset(asset, GetPath(nameParams));
            }
            return asset;
        }
    }
}