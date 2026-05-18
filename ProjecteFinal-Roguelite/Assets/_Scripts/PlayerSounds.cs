using Roguelite.Systems;
using UnityEngine;

namespace Roguelite.Player
{
    [RequireComponent(typeof(AudioSource))]
    public class PlayerSounds : MonoBehaviour
    {
        [Header("Footsetps")]
        [SerializeField] private float _walkStepInterval = 0.5f;
        [SerializeField] private float sprintStepInterval = 0f;

        private AudioSource _source;

        private float _footstepTimer;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void Start()
        {
            UpdateVolume();
        }

        private void UpdateVolume()
        {
            float masterVolume = PlayerPrefs.GetFloat(OptionSettingsUtils.MasterVolumeKey, OptionSettingsUtils.DefaultMasterVolume);
            float sfxVolume = PlayerPrefs.GetFloat(OptionSettingsUtils.SFXVolumeKey, OptionSettingsUtils.DefaultSFXVolume);

            _source.volume = sfxVolume * masterVolume;
        }

        public void PlayPlayerFootsteps(bool isMoving, bool isSprinting)
        {
            float stepInterval = isSprinting ? sprintStepInterval : _walkStepInterval;

            _footstepTimer -= Time.deltaTime;

            if (isMoving)
            {

                if (_footstepTimer <= 0f)
                {
                    if (!isSprinting)
                    {
                        AudioManager.Instance.PlaySound(SoundType.steps, 0.2f);
                    }
                    else
                    {
                        AudioManager.Instance.PlaySound(SoundType.steps, 0.2f);
                    }
                    _footstepTimer = stepInterval;
                }
            }
        }

        public void PlayPlayerDash()
        {
            AudioManager.Instance.PlaySound(SoundType.dash, 0.5f);
        }
        public void PlayPlayerDeath()
        {
            AudioManager.Instance.PlaySound(SoundType.impactPlayerGround, 0.2f);
            AudioManager.Instance.PlaySound(SoundType.playerDeath, 0.1f);
        }
    }
}
