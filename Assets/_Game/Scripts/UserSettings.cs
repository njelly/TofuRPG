using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public static class UserSettings
    {
        private const string SFXVolumeKey = "sfx_volume";
        private const string MusicVolumeKey = "music_volume";
        private const string PlayerItemsKey = "items";

        private static Dictionary<string, int> _playerItems;

        public static float SFXVolume
        {
            get => PlayerPrefs.GetFloat(SFXVolumeKey, 1f);
            set => PlayerPrefs.SetFloat(SFXVolumeKey, value);
        }
        
        public static float MusicVolume
        {
            get => PlayerPrefs.GetFloat(MusicVolumeKey, 1f);
            set => PlayerPrefs.SetFloat(MusicVolumeKey, value);
        }

        public static void AddItem(string itemSpecPath, int quantity)
        {
            if (_playerItems == null)
                ReadPlayerItems();

            if (!_playerItems.TryGetValue(itemSpecPath, out var currentQuantity))
                _playerItems.Add(itemSpecPath, quantity);
            else
                _playerItems[itemSpecPath] = currentQuantity + quantity;
        }

        public static void RemoveItem(string itemSpecPath, int quantity = 1)
        {
            if(_playerItems == null)
                ReadPlayerItems();

            if (_playerItems.TryGetValue(itemSpecPath, out var currentQuantity))
            {
                var newAmount = currentQuantity - quantity;
                if (newAmount <= 0)
                    _playerItems.Remove(itemSpecPath);
                else
                    _playerItems[itemSpecPath] = newAmount;
            }
        }

        public static Dictionary<string, int> GetItems()
        {
            if(_playerItems == null)
                ReadPlayerItems();
            
            return new Dictionary<string, int>(_playerItems);
        }

        public static int GetItemCount(string itemSpecPath)
        {
            if(_playerItems == null)
                ReadPlayerItems();

            if (!_playerItems.TryGetValue(itemSpecPath, out var toReturn))
                toReturn = 0;

            return toReturn;
        }

        private static void ReadPlayerItems()
        {
            var serializedPlayerItems = PlayerPrefs.GetString(PlayerItemsKey, string.Empty);
            _playerItems = string.IsNullOrEmpty(serializedPlayerItems) 
                ? new Dictionary<string, int>() 
                : JsonUtility.FromJson<Dictionary<string, int>>(serializedPlayerItems);
        }

        public static void Save()
        {
            if(_playerItems != null)
                PlayerPrefs.SetString(PlayerItemsKey, JsonUtility.ToJson(_playerItems));
            
            PlayerPrefs.Save();
        }
    }
}