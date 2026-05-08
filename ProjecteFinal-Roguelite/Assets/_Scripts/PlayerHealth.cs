using System;
using Roguelite.Interfaces;
using UnityEngine;

namespace Roguelite.Player
{
    public class PlayerHealth : MonoBehaviour, ITargeteable
    {
        [SerializeField] private float _maxHealth = 3;
        private float _health = 3;

        public static event Action OnPlayerDeath;

        public void Start()
        {
            _health = _maxHealth; // Comença la partida amb la vida plena
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
                _health = value;

                if (_health <= 0)
                {
                    _health = 0;
                }
            }
        }

        public void TakeDamage(float damage)
        {
            Health -= damage;
            Debug.Log(Health);
            if (Health <= 0)
            {
                Die();
            }
        }

        public void Die()
        {
            OnPlayerDeath?.Invoke();
        }
    }
}