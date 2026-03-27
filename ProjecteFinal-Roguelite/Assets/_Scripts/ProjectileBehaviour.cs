using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(Rigidbody))]
    public class ProjectileBehaviour : MonoBehaviour
    {

        private Rigidbody2D _rb;
        private Collider2D _collider;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }
    }
}
