using System;
using Roguelite.Helpers;
using UnityEngine;

namespace Roguelite.Behaviours
{
    [RequireComponent(typeof(MoveBehaviour))]
    public class ReturnToInitPosBehaviour : MonoBehaviour
    {
        public event Action<bool> OnReachDestination;

        private MoveBehaviour _mb;

        private Vector2 _initialPosition = Vector2.zero;

        private void Awake()
        {
            _mb = GetComponent<MoveBehaviour>();

            _initialPosition = transform.position;
        }

        public void ReturnToInitialPosition()
        {
            _mb.MoveCharacter(DistanceUtils.GetDirection(transform.position, _initialPosition));

            if ((Vector2)transform.position == _initialPosition) OnReachDestination?.Invoke(true);
            else OnReachDestination?.Invoke(false);
        }
    }
}