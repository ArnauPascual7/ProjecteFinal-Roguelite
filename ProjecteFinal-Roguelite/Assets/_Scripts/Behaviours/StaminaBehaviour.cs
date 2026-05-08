using System.Collections;
using UnityEngine;

namespace Roguelite.Behaviours
{
    public class StaminaBehaviour : MonoBehaviour
    {
        [SerializeField] private int _dashCost = 50;
        [SerializeField] private float _baseMaxStamina = 100f;
        [SerializeField] private float _baseRegenerationTime = 5f;
        [SerializeField] private float _regenerationStaminaCooldown = 2f;
        [SerializeField] private float _currentStamina;

        private float currentMaxStamina;
        private float currentRegenTime;
        private bool _regenerate = false;
        private Coroutine _coroutine = null;
        private float _timer = 0f;
        private float _regenerationMultiplier = 1f;

        private void Awake()
        {
            _currentStamina = _baseMaxStamina;
        }

        public bool HasStamina()
        {
            return _currentStamina >= _dashCost;
        }

        public void ConsumeStamina(float cooldown)
        {
            _currentStamina -= _dashCost;
            _timer = Time.time + _regenerationStaminaCooldown + cooldown;

            CancelRegeneration();
        }

        public void RegenerateStamina(float cooldown, float multiplier = 1f)
        {
            _regenerationMultiplier = multiplier;

            if (Time.time >= _timer && _currentStamina < _baseMaxStamina)
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
                _currentStamina += ((_baseMaxStamina / _baseRegenerationTime) * Time.deltaTime) * _regenerationMultiplier;
                
                if (_currentStamina >= _baseMaxStamina)
                {
                    _currentStamina = _baseMaxStamina;
                    CancelRegeneration();
                }

                yield return new WaitForSeconds(Time.deltaTime) ;
            }
        }

        public void SetMaxStamina(float newValue)
        {
            currentMaxStamina = newValue;
        }

        public void UpdateRegenRate(float percentageReduction)
        {
            currentRegenTime = _baseRegenerationTime * (1f + (percentageReduction / 100f));
        }
    }
}