using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MoveBehaviour : MonoBehaviour
    {
        [SerializeField] private float baseSpeed = 5f;
        private float currentSpeed;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            _rb.gravityScale = 0f;
            _rb.freezeRotation = true;
        }

        public void MoveCharacter(Vector2 direction)
        {
            _rb.linearVelocity = direction * currentSpeed;
        }

        public void UpdateBaseSpeed(float percentageIncrease)
        {
            // Càlcul: 5 * (1 + 10/100) = 5.5
            currentSpeed = baseSpeed * (1f + (percentageIncrease / 100f));
            Debug.Log($"Nova velocitat: {currentSpeed}");
        }
    }
}