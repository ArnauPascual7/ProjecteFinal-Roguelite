using System;
using Roguelite.Interfaces;
using UnityEngine;

namespace Roguelite.Player
{
    public class PlayerHealth : MonoBehaviour, ITargeteable
    {
        [SerializeField] private float _health = 3;

        public event Action<float> OnHealthChange;
        public event Action OnPlayerDeath;

        public bool IsAlive => _health > 0;

        public float Health {
            get => _health; 
            set
            {
                if (value <= 0) _health = 0;
                else _health = value;

                OnHealthChange?.Invoke(_health);
            }
        }

        private void Start()
        {
            OnHealthChange?.Invoke(_health);
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
        }
    }
}