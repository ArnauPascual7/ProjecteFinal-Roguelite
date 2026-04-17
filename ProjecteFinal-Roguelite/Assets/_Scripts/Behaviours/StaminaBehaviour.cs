using System.Collections;
using UnityEngine;

namespace Roguelite.Behaviours
{
    public class StaminaBehaviour : MonoBehaviour
    {
        [SerializeField] private int _dashCost = 50;
        [SerializeField] private float _maxStamina = 100f;
        [SerializeField] private float _regenerationTime = 5f;
        [SerializeField] private float _regenerationStaminaCooldown = 2f;
        [SerializeField] private float _currentStamina;

        private bool _regenerate = false;
        private Coroutine _coroutine = null;
        private float _timer = 0f;
        private float _regenerationMultiplier = 1f;

        private void Awake()
        {
            _currentStamina = _maxStamina;
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

            if (Time.time >= _timer && _currentStamina < _maxStamina)
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
                _currentStamina += ((_maxStamina / _regenerationTime) * Time.deltaTime) * _regenerationMultiplier;
                
                if (_currentStamina >= _maxStamina)
                {
                    _currentStamina = _maxStamina;
                    CancelRegeneration();
                }

                yield return new WaitForSeconds(Time.deltaTime) ;
            }
        }
    }
}