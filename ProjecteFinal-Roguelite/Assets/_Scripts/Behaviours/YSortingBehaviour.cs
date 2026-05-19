using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class YSortingBehaviour : MonoBehaviour
    {
        [SerializeField] private float _yOffset = 0f;
        [SerializeField] private int _sortingOrderBase = 0;

        private SpriteRenderer _renderer;
        private const float _sortingPrecision = 100f;

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void LateUpdate()
        {
            _renderer.sortingOrder = _sortingOrderBase
                - Mathf.RoundToInt((transform.position.y + _yOffset) * _sortingPrecision);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(new Vector3(0, transform.position.y + _yOffset), .1f);
        }
    }
}