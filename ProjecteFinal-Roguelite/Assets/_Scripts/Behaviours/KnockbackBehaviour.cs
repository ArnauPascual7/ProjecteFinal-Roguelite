using System.Collections;
using UnityEngine;

namespace Roguelite
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class KnockbackBehaviour : MonoBehaviour
    {
        [SerializeField] private float _knockbackResistance = 0f;
        [SerializeField] private float _knockbackDuration = 0.2f;
        [SerializeField] private float _knockbackRecoveryTime = 0.1f;

        public bool IsReceivingKnockback { get; private set; }

        private Rigidbody2D _rb;
        private bool canReceiveKnockback = true;
        private Coroutine _knockbackCoroutine = null;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void Knockback(Vector2 direction, float force)
        {
            if (canReceiveKnockback && !IsReceivingKnockback)
            {
                _knockbackCoroutine ??= StartCoroutine(KnockbackCoroutine(direction, force));
            }
        }

        public IEnumerator KnockbackCoroutine(Vector2 direction, float force)
        {
            canReceiveKnockback = false;
            IsReceivingKnockback = true;

            _rb.AddForce((1f - _knockbackResistance) * force * direction.normalized, ForceMode2D.Impulse);

            yield return new WaitForSeconds(_knockbackDuration);
            IsReceivingKnockback = false;
            _rb.linearVelocity = Vector2.zero;

            yield return new WaitForSeconds(_knockbackRecoveryTime);
            canReceiveKnockback = true;

            _knockbackCoroutine = null;
        }
    }
}
