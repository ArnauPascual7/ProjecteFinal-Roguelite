using System;
using Roguelite.Interfaces;
using UnityEngine;

namespace Roguelite.Enemy
{
    public class EnemyHealth : MonoBehaviour, ITargeteable
    {
        [SerializeField] private float _health = 100;

        public float Health
        {
            get => _health;
            set => _health = value;
        }

        public event Action<bool> OnEnemyDeath;


        public void TakeDamage(float damage)
        {
            _health -= damage;

            if (_health <= 0)
            {
                Die();
            }
        }
        public void Die()
        {
            OnEnemyDeath?.Invoke(true);
        }
    }
}