using System;
using Roguelite.Interfaces;
using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class MeleeAttackBehaviour : MonoBehaviour
    {
        [SerializeField] private float _attackCooldown = 1f;

        public event Action<bool> OnCanAttack;

        [Tooltip("This field is procedurally initialized")]
        public GameObject target;
        public float damage = 1f;

        private BoxCollider2D _collider;

        private float _timer;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _collider.isTrigger = false;

            _timer = Time.time;
        }

        public void Attack()
        {
            if (Time.time > _timer + _attackCooldown)
            {
                if (target.TryGetComponent<ITargeteable>(out ITargeteable targetable))
                {
                    targetable.TakeDamage(damage);
                    Debug.Log($"MELEE ATTACK BEHAVIOUR: Attacking, {damage} damage to {target.name}");
                }

                _timer = Time.time;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (target != null)
            {
                if (collision.gameObject.layer == target.layer)
                {
                    OnCanAttack?.Invoke(true);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (target != null)
            {
                if (collision.gameObject.layer == target.layer)
                {
                    OnCanAttack?.Invoke(false);
                }
            }
        }
    }
}