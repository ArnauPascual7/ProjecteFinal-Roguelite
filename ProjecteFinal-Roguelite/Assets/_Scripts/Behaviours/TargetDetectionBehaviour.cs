using System;
using Roguelite.Helpers;
using UnityEngine;

namespace Roguelite.Behaviours
{
    public class TargetDetectionBehaviour : MonoBehaviour
    {
        [SerializeField] private float _detectionRange = 10f;
        [SerializeField] private float _detectionTickRate = 0.1f;
        [Tooltip("By default adds gameObject and Target Layers")]
        [SerializeField] private LayerMask _ignoreLayers;

        public event Action<bool> OnTargetDetected;

        [Tooltip("This field is procedurally initialized")]
        public GameObject target;

        private LayerMask _targetLayerMask;
        private bool _inRange;

        private void Start()
        {
            _ignoreLayers |= (1 << gameObject.layer) | (1 << target.layer) | (1 << GetComponentInChildren<Transform>().gameObject.layer);
            _targetLayerMask = 1 << target.layer;

            InvokeRepeating(nameof(DetectionTick), 0f, _detectionTickRate);
        }

        private void DetectionTick()
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, _detectionRange, _targetLayerMask);

            bool detected = hit != null &&
                DistanceUtils.HasLineOfSight(
                    transform.position,
                    target.transform.position,
                    _detectionRange,
                    _ignoreLayers
                );

            if (detected == _inRange) return;

            _inRange = detected;
            OnTargetDetected?.Invoke(_inRange);
        }

        private void OnDestroy()
        {
            CancelInvoke(nameof(DetectionTick));
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _detectionRange);
        }
    }
}