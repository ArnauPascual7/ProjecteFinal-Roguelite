using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ProjectileBehaviour : MonoBehaviour
    {

        private Rigidbody2D _rb;
        public float speed = 10f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Destroy(gameObject);
        }
        private void FixedUpdate()
        {
            _rb.MovePosition(transform.position + transform.right * speed * Time.fixedDeltaTime);
        }
    }
}
