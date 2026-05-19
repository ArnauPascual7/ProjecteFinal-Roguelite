using Roguelite.Helpers;
using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(MoveBehaviour))]
    public class ChaseBehaviour : MonoBehaviour
    {
        private MoveBehaviour _mb;
        private bool _moving = true;
        private void Awake()
        {
            _mb = GetComponent<MoveBehaviour>();
        }

        public void ChaseTarget(Transform target)
        {
            if (_moving)
                _mb.MoveCharacter(DistanceUtils.GetDirection(transform.position, target.position));
        }

        public void StopChase()
        {
            _mb.MoveCharacter(Vector2.zero);
            _moving = false;
            
        }
        public void ResumeChasing()
        {
            _moving = true;
        }
    }
}