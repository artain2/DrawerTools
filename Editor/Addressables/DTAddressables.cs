using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;
namespace DrawerTools
{

    public static class DTAddressables
    {
        // Get Address
        //_____________________________________________________
        public static bool TryGetAddressableAddress(Object asset, out string address)
        {
            if (!TryFindAddressableAssetEntry(asset, out var entry))
            {
                address = "";
                return false;
            }
            address = entry.address;
            return true;
        }
        public static string GetAddressableAddress(Object asset)
        {
            TryGetAddressableAddress(asset, out var result);
            return result;
        }

        // Set Address
        //_____________________________________________________
        public static bool SetAddressableAddress(Object asset, string address)
        {
            if (string.IsNullOrEmpty(address))
            {
                Debug.LogError($"Can not set an empty adressables ID.");
                return false;
            }
            if (TryFindAddressableAssetEntry(asset, out var entry))
            {
                entry.address = address;
                return true;
            }
            return false;
        }

        // Set as Addressable
        //_____________________________________________________
        public static void SetObjectAddressable(Object asset, string address, string groupName = null, string label = null)
        {
            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            AddressableAssetGroup group = settings.DefaultGroup;
            if (groupName != null)
            {
                group = settings.FindGroup(groupName);
                if (group == null)
                {
                    Debug.LogError($"Не найдена группа {groupName}, объект не будет помечен как Addressable");
                    return;
                }
            }
            bool assetFound = TryGetAssetGUID(asset, out var guid);
            if (!assetFound)
            {
                return;
            }

            var entry = settings.CreateOrMoveEntry(guid, group);
            entry.address = address;
            if (label != null)
            {
                entry.labels.Add(label);
            }
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
        }

        public static void SetObjectNotAddressable(Object asset)
        {
            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            var group = settings.DefaultGroup;
            bool assetFound = TryGetAssetGUID(asset, out var guid);
            if (!assetFound)
            {
                return;
            }

            var entry = settings.RemoveAssetEntry(guid, group);
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);
        }

        private static bool TryFindAddressableAssetEntry(Object asset, out AddressableAssetEntry entry)
        {
            entry = null;
            bool assetFound = TryGetAssetGUID(asset, out var guid);
            if (!assetFound)
            {
                return false;
            }

            var found = TryFindAddressableAssetEntry(guid, out entry);
            return found;
        }

        private static bool TryFindAddressableAssetEntry(string guid, out AddressableAssetEntry entry)
        {
            entry = null;
            if (!IsAssetInAssets(guid))
            {
                return false;
            }
            AddressableAssetSettings aaSettings = AddressableAssetSettingsDefaultObject.Settings;
            if (aaSettings == null)
            {
                return false;
            }
            entry = aaSettings.FindAssetEntry(guid);
            return entry != null;
        }

        private static bool TryGetAssetGUID(Object asset, out string guid)
        {
            bool assetFound = AssetDatabase.TryGetGUIDAndLocalFileIdentifier(asset, out guid, out long _);
            return assetFound;
        }

        private static bool IsAssetInAssets(string guid)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            return path.ToLower().Contains("assets");
        }
    }
}