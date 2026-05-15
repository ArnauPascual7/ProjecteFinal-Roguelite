using Roguelite.Behaviours;
using UnityEngine;

namespace Roguelite.Enemy
{
    public class EnemyWeapon : WeaponController
    {
        public float offset = 0.2f;

        private ProjectileFiringBehaviour _pfb;
        private AimRotationBehaviour _arb;
        private TargetDetectionBehaviour _tdb;

        private Vector2 _initialPosition;

        protected override void Awake()
        {
            _pfb = GetComponent<ProjectileFiringBehaviour>();
            _arb = GetComponent<AimRotationBehaviour>();
            _tdb = GetComponent<TargetDetectionBehaviour>();

            _initialPosition = shootPoint.transform.localPosition;

            base.Awake();
        }

        public void AimPosition()
        {
            Vector2 direction = _arb.GetDirectionTowardsTarget(_tdb.target.transform.position);
            float angle = _arb.GetAngleTowardsTarget(_tdb.target.transform.position);

            shootPoint.transform.SetPositionAndRotation((Vector2)transform.position + direction.normalized * offset, Quaternion.Euler(0f, 0f, angle));
        }

        public void ResetPosition()
        {
            shootPoint.transform.localPosition = _initialPosition;
            shootPoint.transform.localRotation = Quaternion.identity;
        }
    }
}
