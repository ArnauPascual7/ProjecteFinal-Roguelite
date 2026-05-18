using System;
using Roguelite.Enemy;
using Roguelite.Helpers;
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
                    GetComponent<ChaseBehaviour>().StopChase();

                }
                if (collision.gameObject.layer == gameObject.layer)
                {
                    if (DistanceUtils.GetDistance(collision.gameObject.transform.position, target.transform.position) <
                        DistanceUtils.GetDistance(transform.position, target.transform.position))
                    {
                        GetComponent<ChaseBehaviour>().StopChase();
                    }
                }
            }
        }
        
        /*private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.layer == gameObject.layer)
            {
                GetComponent<ChaseBehaviour>().StopChase();
            }
        }*/

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (target != null)
            {
                if (collision.gameObject.layer == target.layer)
                {
                    GetComponent<ChaseBehaviour>().ResumeChasing();
                    OnCanAttack?.Invoke(false);
                }
                if (collision.gameObject.layer == gameObject.layer)
                {
                        GetComponent<ChaseBehaviour>().ResumeChasing();
                }
            }
        }
    }
}