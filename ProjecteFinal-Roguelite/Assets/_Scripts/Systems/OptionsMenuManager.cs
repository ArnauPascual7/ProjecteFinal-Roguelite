using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Roguelite.Systems
{
    public class OptionsMenuManager : MonoBehaviour
    {
        [Header("Reference")]
        [SerializeField] private GameObject _optionsMenu;

        [Header("Option Menu References")]
        [SerializeField] private Slider _optSensitivitySlider;
        [SerializeField] private Slider _optMasterVolSlider;
        [SerializeField] private Slider _optMusicVolSlider;
        [SerializeField] private Slider _optSFXVolSlider;
        [SerializeField] private TextMeshProUGUI _optSensitivityValueText;
        [SerializeField] private TextMeshProUGUI _optMasterVolValueText;
        [SerializeField] private TextMeshProUGUI _optMusicVolValueText;
        [SerializeField] private TextMeshProUGUI _optSFXVolValueText;

        public static event Action OnOptionsChange;

        private float _currentSensitivity;
        private float _currentMasterVolume;
        private float _currentMusicVolume;
        private float _currentSFXVolume;

        private GameObject _returnMenu;

        public void ShowOptionsMenu(GameObject currentMenu)
        {
            _returnMenu = currentMenu;

            currentMenu.SetActive(false);
            _optionsMenu.SetActive(true);

            SetOptionsMenu();
        }
        
        private void SetOptionsMenu()
        {
            _currentSensitivity = PlayerPrefs.GetFloat(OptionSettingsUtils.SensitivityKey, OptionSettingsUtils.DefaultSensitivity);
            _currentMasterVolume = PlayerPrefs.GetFloat(OptionSettingsUtils.MasterVolumeKey, OptionSettingsUtils.DefaultMasterVolume);
            _currentMusicVolume = PlayerPrefs.GetFloat(OptionSettingsUtils.MusicVolumeKey, OptionSettingsUtils.DefaultMusicVolume);
            _currentSFXVolume = PlayerPrefs.GetFloat(OptionSettingsUtils.SFXVolumeKey, OptionSettingsUtils.DefaultSFXVolume);

            _optSensitivitySlider.value = _currentSensitivity;
            _optMasterVolSlider.value = _currentMasterVolume;
            _optMusicVolSlider.value = _currentMusicVolume;
            _optSFXVolSlider.value = _currentSFXVolume;

            UpdateSensSliderText();
            UpdateMasterVolSliderText();
            UpdateMusicVolSliderText();
            UpdateSFXVolSliderText();
        }

        public void UpdateSensSliderText()
        {
            _currentSensitivity = _optSensitivitySlider.value;
            _optSensitivityValueText.text = (_currentSensitivity * 10).ToString("0.0");
        }

        public void UpdateMasterVolSliderText()
        {
            _currentMasterVolume = _optMasterVolSlider.value;
            _optMasterVolValueText.text = (_currentMasterVolume * 100).ToString("0");
        }

        public void UpdateMusicVolSliderText()
        {
            _currentMusicVolume = _optMusicVolSlider.value;
            _optMusicVolValueText.text = (_currentMusicVolume * 100).ToString("0");
        }

        public void UpdateSFXVolSliderText()
        {
            _currentSFXVolume = _optSFXVolSlider.value;
            _optSFXVolValueText.text = (_currentSFXVolume * 100).ToString("0");
        }

        public void ResetOptions()
        {
            _currentSensitivity = OptionSettingsUtils.DefaultSensitivity;
            _currentMasterVolume = OptionSettingsUtils.DefaultMasterVolume;
            _currentMusicVolume = OptionSettingsUtils.DefaultMusicVolume;
            _currentSFXVolume = OptionSettingsUtils.DefaultSFXVolume;

            _optSensitivitySlider.value = OptionSettingsUtils.DefaultSensitivity;
            _optMasterVolSlider.value = OptionSettingsUtils.DefaultMasterVolume;
            _optMusicVolSlider.value = OptionSettingsUtils.DefaultMusicVolume;
            _optSFXVolSlider.value = OptionSettingsUtils.DefaultSFXVolume;
        }

        public void SaveOptions()
        {
            PlayerPrefs.SetFloat(OptionSettingsUtils.SensitivityKey, _currentSensitivity);
            PlayerPrefs.SetFloat(OptionSettingsUtils.MasterVolumeKey, _currentMasterVolume);
            PlayerPrefs.SetFloat(OptionSettingsUtils.MusicVolumeKey, _currentMusicVolume);
            PlayerPrefs.SetFloat(OptionSettingsUtils.SFXVolumeKey, _currentSFXVolume);

            OnOptionsChange?.Invoke();
        }

        public void ExitOptions()
        {
            SaveOptions();
            _optionsMenu.SetActive(false);
            _returnMenu.SetActive(true);
        }
    }
}
