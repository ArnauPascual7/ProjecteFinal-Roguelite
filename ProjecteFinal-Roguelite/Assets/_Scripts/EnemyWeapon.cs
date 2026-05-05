using Roguelite.Behaviours;
using UnityEngine;

namespace Roguelite.Enemy
{
    public class EnemyWeapon : WeaponController
    {
        private ProjectileFiringBehaviour _pfb;
        private AimRotationBehaviour _arb;
        private TargetDetectionBehaviour _tdb;

        protected override void Awake()
        {
            _pfb = GetComponent<ProjectileFiringBehaviour>();
            _arb = GetComponent<AimRotationBehaviour>();
            _tdb = GetComponent<TargetDetectionBehaviour>();

            base.Awake();
        }

        private void Update()
        {
            AimPosition();
        }

        private void AimPosition()
        {
            Vector2 direction = _arb.GetDirectionTowardsTarget(_tdb.target.transform.position);
            float angle = _arb.GetAngleTowardsTarget(_tdb.target.transform.position);

            shootPoint.transform.SetPositionAndRotation((Vector2)transform.position + direction.normalized * 1f, Quaternion.Euler(0f, 0f, angle));
        }
    }
}
