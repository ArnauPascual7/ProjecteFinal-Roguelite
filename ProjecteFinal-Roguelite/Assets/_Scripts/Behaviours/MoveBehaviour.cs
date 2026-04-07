using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MoveBehaviour : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            _rb.gravityScale = 0f;
        }

        public void MoveCharacter(Vector2 direction)
        {
            _rb.linearVelocity = direction * speed;
        }
    }
}