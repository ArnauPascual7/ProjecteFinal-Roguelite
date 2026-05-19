using System;
using Roguelite.Helpers;
using Roguelite.Interfaces;
using UnityEngine;

namespace Roguelite.Behaviours
{
    public class MeleeAttackBehaviour : MonoBehaviour
    {
        [SerializeField] private float _attackCooldown = 1f;
        [SerializeField] private float _attackTickRate = 0.1f;
        [SerializeField] private Vector2 _attackBoxSize = Vector2.one;
        [SerializeField] private float _attackBoxYOffset = 0f;
        
        public event Action<bool> OnCanAttack;

        [Tooltip("This field is procedurally initialized")]
        public GameObject target;
        public float damage = 1f;

        private LayerMask _targetLayerMask;
        private LayerMask _gameobjectLayerMask;

        private float _lastAttackTime;
        private bool _canAttack;
        private ChaseBehaviour _cb;

        private void Awake()
        {
            _cb = GetComponent<ChaseBehaviour>();
        }

        private void Start()
        {
            _targetLayerMask = 1 << target.layer;
            _gameobjectLayerMask = 1 << gameObject.layer;

            InvokeRepeating(nameof(AttackTick), 0f, _attackTickRate);
        }

        public void SetAttackBox(Vector2 size, float yOffset)
        {
            _attackBoxSize = size;
            _attackBoxYOffset = yOffset;
        }

        private void AttackTick()
        {
            bool targetInRange = Physics2D.OverlapBox(
                transform.position + new Vector3(0, _attackBoxYOffset),
                _attackBoxSize,
                0f,
                _targetLayerMask
            ) != null;

            if (targetInRange != _canAttack)
            {
                _canAttack = targetInRange;
                OnCanAttack?.Invoke(_canAttack);

                if (_canAttack)
                    _cb.StopChase();
                else
                    _cb.ResumeChasing();
            }

            if (!_canAttack)
            {
                Collider2D blockingEnemy = Physics2D.OverlapBox(
                    transform.position,
                    _attackBoxSize,
                    0f,
                    _gameobjectLayerMask
                );

                if (blockingEnemy != null)
                {
                    float blockerDist = DistanceUtils.GetDistance(
                        blockingEnemy.transform.position,
                        target.transform.position
                    );
                    float selfDist = DistanceUtils.GetDistance(
                        transform.position,
                        target.transform.position
                    );

                    if (blockerDist < selfDist)
                        _cb.StopChase();
                    else
                        _cb.ResumeChasing();
                }
            }
        }

        public void Attack()
        {
            if (Time.time < _lastAttackTime + _attackCooldown) return;

            if (target.TryGetComponent<ITargeteable>(out ITargeteable targetable))
            {
                targetable.TakeDamage(damage);
                Debug.Log($"MELEE ATTACK BEHAVIOUR: Attacking, {damage} damage to {target.name}");
            }

            if (target.TryGetComponent<KnockbackBehaviour>(out var kb))
            {
                kb.Knockback(DistanceUtils.GetDirection(gameObject.transform.position, target.transform.position));
            }

            _lastAttackTime = Time.time;
        }

        private void OnDestroy()
        {
            CancelInvoke(nameof(AttackTick));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + new Vector3(0, _attackBoxYOffset), _attackBoxSize);
        }
    }
}