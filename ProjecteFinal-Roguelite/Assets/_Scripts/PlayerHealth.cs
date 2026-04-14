using System;
using Roguelite.Interfaces;
using UnityEngine;

namespace Roguelite.Player
{
    public class PlayerHealth : MonoBehaviour, ITargeteable
    {
        [SerializeField] private float _health = 3;

        public static event Action OnPlayerDeath;

        public float Health {
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