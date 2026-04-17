using Roguelite.Interfaces;
using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileBehaviour : MonoBehaviour
    {
        [HideInInspector] public ProjectileFiringBehaviour shooter;

        private Rigidbody2D _rb;
        public float speed = 10f;
        public float damage = 10f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer != shooter.gameObject.layer && collision.gameObject.layer != gameObject.layer)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer(shooter.targetLayerName))
                {
                    if (collision.gameObject.TryGetComponent<ITargeteable>(out ITargeteable target))
                    {
                        target.TakeDamage(damage);
                    }
                }
            }
            Destroy(gameObject);
        }
        private void FixedUpdate()
        {
            _rb.MovePosition(transform.position + transform.right * speed * Time.fixedDeltaTime);
        }
    }
}
