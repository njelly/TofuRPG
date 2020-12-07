using UnityEngine;

namespace Tofunaut.TofuRPG
{
    public static class UserSettings
    {
        private const string SFXVolumeKey = "sfx_volume";
        private const string MusicVolumeKey = "music_volume";

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

        public static void Save()
        {
            PlayerPrefs.Save();
        }
    }
}