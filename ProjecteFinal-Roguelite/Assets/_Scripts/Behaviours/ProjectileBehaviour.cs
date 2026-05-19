using Roguelite.Interfaces;
using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileBehaviour : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private ProjectileFiringBehaviour _shooter;
        private GameObject _prefabKey;
        private float _speed;
        private float _damage;
        private float _force;
        private float _range;
        private Vector2 _direction;
        private Vector2 _shotPosition;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            if (_rb != null) _rb.linearVelocity = Vector2.zero;
        }

        public void Initialize(ProjectileFiringBehaviour shooter, GameObject prefabKey, Transform shootPoint,
                               float speed, float damage, float force, float range, string layer)
        {
            _shooter = shooter;
            _prefabKey = prefabKey;
            _speed = speed;
            _damage = damage;
            _force = force;
            _range = range;
            _direction = (shootPoint.position - shooter.transform.position).normalized;
            _shotPosition = shooter.transform.position;

            gameObject.layer = LayerMask.NameToLayer(layer);
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _direction * _speed;

            if (Vector2.Distance(transform.position, _shotPosition) >= _range)
                ReturnToPool();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject == _shooter.gameObject ||
                collision.gameObject.layer == gameObject.layer) return;

            if (collision.gameObject.TryGetComponent(out ITargeteable target))
                target.TakeDamage(_damage);

            if (collision.gameObject.TryGetComponent(out KnockbackBehaviour knockback))
                knockback.Knockback(_direction, _force);

            ReturnToPool();
        }

        private void ReturnToPool()
        {
            _shooter.ReturnToPool(_prefabKey, gameObject);
        }
    }
}