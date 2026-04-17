using System.Collections;
using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DashBehaviour : MonoBehaviour
    {
        [SerializeField] private float _dashSpeed = 20f;
        [SerializeField] private float _dashDuration = 0.10f;
        [SerializeField] private float _dashCooldown = 2.0f;

        public float DashCooldown => _dashCooldown;
        public bool IsDashing { get; private set; } = false;
        public bool CanDash { get; private set; } = true;

        private Rigidbody2D _rb;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0f;
        }

        public void Dash(Vector2 direction)
        {
            if (CanDash && !IsDashing)
                StartCoroutine(OnDash(direction));
        }

        private IEnumerator OnDash(Vector2 direction)
        {
            CanDash = false;
            IsDashing = true;

            _rb.linearVelocity = direction.normalized * _dashSpeed;

            yield return new WaitForSeconds(_dashDuration);

            _rb.linearVelocity = Vector2.zero;
            IsDashing = false;

            yield return new WaitForSeconds(_dashCooldown);
            CanDash = true;
        }
    }
}