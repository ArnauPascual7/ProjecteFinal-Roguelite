using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MoveBehaviour : MonoBehaviour
    {
        [SerializeField] private float _baseSpeed = 5f;
        private float _currentSpeed;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

            _rb.gravityScale = 0f;
            _rb.freezeRotation = true;

            _currentSpeed = _baseSpeed;
        }
        //
        public float Speed
        {
            get => _currentSpeed;
            set => _currentSpeed = value;
        }
        //
        public void MoveCharacter(Vector2 direction)
        {
            _rb.linearVelocity = direction * _currentSpeed;
        }

        public void UpdateBaseSpeed(float percentageIncrease)
        {
            // Càlcul: 5 * (1 + 10/100) = 5.5
            _currentSpeed = _baseSpeed * (1f + (percentageIncrease / 100f));
            Debug.Log($"Nova velocitat: {_currentSpeed}");
        }

        
    }
}