using System;
using Roguelite.Helpers;
using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class TargetDetectionBehaviour : MonoBehaviour
    {
        [SerializeField] private float _detectionRange = 10f;

        [Tooltip("By default adds gameObject and Target Layers")]
        [SerializeField] private LayerMask _ignoreLayers;

        public event Action<bool> OnTargetDetected;

        [Tooltip("This field is procedurally initialized")]
        public GameObject target;

        private CircleCollider2D _collider;

        private bool _inRange;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _collider.isTrigger = true;
            _collider.radius = _detectionRange;
        }

        private void Start()
        {
            _ignoreLayers |= (1 << gameObject.layer) | (1 << target.layer);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.layer == target.layer)
            {
                _inRange = true;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_inRange && DistanceUtils.HasLineOfSight(transform.position, target.transform.position, _collider.radius, _ignoreLayers))
            {
                OnTargetDetected?.Invoke(true);
            }
            else
            {
                OnTargetDetected?.Invoke(false);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.layer == target.layer)
            {
                OnTargetDetected?.Invoke(false);
                _inRange = false;
            }
        }
    }
}