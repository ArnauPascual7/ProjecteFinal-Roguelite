using System.Collections;
using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DashBehaviour : MonoBehaviour
    {
        [SerializeField] private float dashSpeed = 20f;
        [SerializeField] private float dashDuration = 0.10f;
        [SerializeField] private float dashCooldown = 2.0f;

        public bool IsDashing { get; private set; }

        private Rigidbody2D _rb;
        private bool canDash = true;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0f;
        }

        public void OnDash(Vector2 direction)
        {
            if (canDash && !IsDashing)
                StartCoroutine(Dash(direction));
        }

        IEnumerator Dash(Vector2 direction)
        {
            canDash = false;
            IsDashing = true;

            _rb.linearVelocity = direction.normalized * dashSpeed;

            yield return new WaitForSeconds(dashDuration);

            _rb.linearVelocity = Vector2.zero;
            IsDashing = false;

            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }
    }
}