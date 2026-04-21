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
        private Vector2 _direction;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize(ProjectileFiringBehaviour shooter, Transform shootPoint, float speed, float damage)
        {
            _shooter = shooter;
            _speed = speed;
            _damage = damage;
            _direction = (shootPoint.transform.position - shooter.gameObject.transform.position).normalized;

            transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);

            gameObject.layer = shooter.gameObject.layer;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer != _shooter.gameObject.layer && collision.gameObject.layer != gameObject.layer)
            {
                if (collision.gameObject.TryGetComponent(out ITargeteable target))
                {
                    target.TakeDamage(_damage);
                }

                Destroy(gameObject);
            }
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _direction * _speed;
        }
    }
}
