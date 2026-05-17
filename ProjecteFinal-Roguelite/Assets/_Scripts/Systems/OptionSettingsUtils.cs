using UnityEngine;

namespace Roguelite
{
    public static class OptionSettingsUtils
    {
        public static float DefaultSensitivity { get => _defaultSensitivity; }
        public static float DefaultMasterVolume { get => _defaultMasterVolume; }
        public static float DefaultMusicVolume { get => _defaultMusicVolume; }
        public static float DefaultSFXVolume { get => _defaultSFXVolume; }

        public static string SensitivityKey { get => _sensitivityKey; }
        public static string MasterVolumeKey { get => _masterVolumeKey; }
        public static string MusicVolumeKey { get => _musicVolumeKey; }
        public static string SFXVolumeKey { get => _sfxVolumeKey; }

        private static readonly float _defaultSensitivity = 0.1f;
        private static readonly float _defaultMasterVolume = 1f;
        private static readonly float _defaultMusicVolume = 1f;
        private static readonly float _defaultSFXVolume = 1f;

        private static readonly string _sensitivityKey = "Sensitivity";
        private static readonly string _masterVolumeKey = "MasterVolume";
        private static readonly string _musicVolumeKey = "MusicVolume";
        private static readonly string _sfxVolumeKey = "SfXVolume";
    }
}
