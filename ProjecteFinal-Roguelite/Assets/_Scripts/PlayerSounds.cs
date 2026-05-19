using Roguelite.Systems;
using UnityEngine;

namespace Roguelite.Player
{
    public class PlayerSounds : MonoBehaviour
    {
        [Header("Footsetps")]
        [SerializeField] private float _walkStepInterval = 0.5f;
        [SerializeField] private float sprintStepInterval = 0f;


        private float _footstepTimer;

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
    }
}
