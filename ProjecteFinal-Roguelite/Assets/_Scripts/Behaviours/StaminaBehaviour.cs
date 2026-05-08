using System.Collections;
using UnityEngine;

namespace Roguelite.Behaviours
{
    public class StaminaBehaviour : MonoBehaviour
    {
        public float currentStamina = 0f;

        [SerializeField] private int _dashCost = 50;
        [SerializeField] private float _maxStamina = 100f;
        [SerializeField] private float _regenerationTime = 5f;
        [SerializeField] private float _regenerationStaminaCooldown = 2f;

        private bool _regenerate = false;
        private Coroutine _coroutine = null;
        private float _timer = 0f;
        private float _regenerationMultiplier = 1f;

        private void Awake()
        {
            currentStamina = _maxStamina;
        }

        public bool HasStamina()
        {
            return currentStamina >= _dashCost;
        }

        public void ConsumeStamina(float cooldown)
        {
            currentStamina -= _dashCost;
            _timer = Time.time + _regenerationStaminaCooldown + cooldown;

            CancelRegeneration();
        }

        public void RegenerateStamina(float cooldown, float multiplier = 1f)
        {
            _regenerationMultiplier = multiplier;

            if (Time.time >= _timer && currentStamina < _maxStamina)
            {
                _regenerate = true;

                _coroutine ??= StartCoroutine(StaminaRegeneration());

                _timer = Time.time + _regenerationStaminaCooldown + cooldown;
            }
        }

        private void CancelRegeneration()
        {
            _regenerate = false;
            
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        private IEnumerator StaminaRegeneration()
        {
            while (_regenerate)
            {
                currentStamina += ((_maxStamina / _regenerationTime) * Time.deltaTime) * _regenerationMultiplier;
                
                if (currentStamina >= _maxStamina)
                {
                    currentStamina = _maxStamina;
                    CancelRegeneration();
                }

                yield return new WaitForSeconds(Time.deltaTime) ;
            }
        }
    }
}