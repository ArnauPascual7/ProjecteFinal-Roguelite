using Roguelite.Behaviours;
using UnityEngine;

namespace Roguelite.Player
{
    [RequireComponent(typeof(PlayerInputs))]
    [RequireComponent(typeof(AimRotationBehaviour), typeof(ProjectileFiringBehaviour))]
    public class PlayerWeapon : WeaponController
    {
        [SerializeField] private Camera _camera;

        private PlayerInputs _playerInputs;

        private AimRotationBehaviour _arb;
        private ProjectileFiringBehaviour _pfb;

        protected override void Awake()
        {
            _arb = GetComponent<AimRotationBehaviour>();
            _pfb = GetComponent<ProjectileFiringBehaviour>();
            _playerInputs = GetComponentInParent<PlayerInputs>();

            base.Awake();
        }

        private void Update()
        {
            AimPosition();
            CheckFiring();
        }

        private void AimPosition()
        {
            Vector2 direction = _arb.GetDirectionTowardsMouse(_camera, _playerInputs.MousePosition);
            float angle = _arb.GetAngleTowardsMouse(_camera, _playerInputs.MousePosition);

            shootPoint.transform.SetPositionAndRotation((Vector2)transform.position + direction.normalized * 1f, Quaternion.Euler(0f, 0f, angle));
        }

        private void CheckFiring()
        {
            if (_playerInputs.AttackInput)
            {
                Shoot();
            }
        }
    }
}