using Roguelite.Helpers;
using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(MoveBehaviour))]
    public class ChaseBehaviour : MonoBehaviour
    {
        private MoveBehaviour _mb;

        private void Awake()
        {
            _mb = GetComponent<MoveBehaviour>();
        }

        public void ChaseTarget(Transform target)
        {
            _mb.MoveCharacter(DistanceUtils.GetDirection(transform.position, target.position));
        }

        public void StopChase()
        {
            _mb.MoveCharacter(Vector2.zero);
        }
    }
}