using Roguelite.Helpers;
using UnityEngine;

namespace Roguelite.Behaviours
{
    public class ChaseBehaviour : MonoBehaviour
    {
        [SerializeField] private float _speed = 2f;
        
        public void ChaseTarget(Transform target)
        {
            Vector2 direction = DistanceUtils.GetDirection(transform.position, target.position);
            transform.Translate(direction * _speed * Time.deltaTime);
        }
    }
}