using Roguelite.Interfaces;
using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileBehaviour : MonoBehaviour
    {
        private Rigidbody2D _rb;
        
        private ProjectileFiringBehaviour _shooter;
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

        public void Initialize(ProjectileFiringBehaviour shooter, Transform shootPoint, float speed, float damage, float force, float range, string layer)
        {
            _shooter = shooter;
            _speed = speed;
            _damage = damage;
            _force = force;
            _range = range;
            _direction = (shootPoint.transform.position - shooter.gameObject.transform.position).normalized;
            _shotPosition = shooter.transform.position;

            transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);
            
            gameObject.layer = LayerMask.NameToLayer(layer);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer != _shooter.gameObject.layer && collision.gameObject.layer != gameObject.layer)
            {
                if (collision.gameObject.TryGetComponent(out ITargeteable target))
                {
                    target.TakeDamage(_damage);
                }

                if (collision.gameObject.TryGetComponent(out KnockbackBehaviour knockback))
                {
                    Vector2 direction = ((Vector2)transform.position - _shotPosition).normalized;
                    knockback.Knockback(direction, _force);
                }

                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _direction * _speed;

            if (Vector2.Distance(transform.position, _shotPosition) >= _range)
            {
                Destroy(gameObject);
            }
        }
    }
}
