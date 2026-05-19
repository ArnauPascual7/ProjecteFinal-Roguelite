using Roguelite.Interfaces;
using Roguelite.Systems;
using System;
using UnityEngine;

namespace Roguelite.Player
{
    public class PlayerHealth : MonoBehaviour, ITargeteable
    {
        [SerializeField] private float _health = 3;
        [SerializeField] private float _maxHealth = 3;

        public event Action<float> OnHealthChange;
        public event Action<float> OnMaxHealthChange;
        public event Action OnPlayerDeath;

        public bool IsAlive => _health > 0;

        public void Start()
        {
            _health = _maxHealth; // Comença la partida amb la vida plena
            OnMaxHealthChange?.Invoke(_maxHealth);
            OnHealthChange?.Invoke(_health);
        }

        public void SetMaxHealth(float newMax)
        {
            _maxHealth = newMax;
            _health = _maxHealth;
        }

        public float Health
        {
            get => _health;
            set
            {
                _health = Mathf.Clamp(value, 0, _maxHealth);
                OnHealthChange?.Invoke(_health);
            }
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            OnPlayerDeath?.Invoke();
            AudioManager.Instance.PlaySound(SoundType.impactPlayerGround, 0.6f);
            AudioManager.Instance.PlaySound(SoundType.playerDeath, 0.5f);
        }
    }
}