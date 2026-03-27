using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(Rigidbody))]
    public class MoveBehaviour : MonoBehaviour
    {
        [SerializeField] private float speed;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void MoveCharacter(Vector2 direction)
        {
            _rb.linearVelocity = new Vector2(direction.x * speed, _rb.linearVelocityY);
        }
    }
}