using System.Collections;
using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent (typeof(Rigidbody2D))]
    public class DashBehaviour : MonoBehaviour
    {
        [SerializeField] private float dashSpeed = 20f;
        [SerializeField] private float dashDuration = 0.8f;
        [SerializeField] private float dashCooldown = 1.0f;

        private Rigidbody2D _rb;

        private bool canDash = true;
        private bool isDashing;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();

        }
        public void OnDash(Vector2 direction)
        {
            if (canDash && !isDashing)
            {
                StartCoroutine(Dash(direction));

            }
        } 
        IEnumerator Dash(Vector2 direction)
        {
            canDash = false;
            isDashing = true;

            float originalGravity = _rb.gravityScale;
            _rb.gravityScale = 0f;

            float dashDirection = direction.x;
            _rb.linearVelocityX = dashDirection * dashSpeed;
            Debug.Log(_rb.linearVelocityX);
            yield return new WaitForSeconds(dashDuration);
            _rb.gravityScale = originalGravity;
            isDashing = false;

            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }

    }
}
