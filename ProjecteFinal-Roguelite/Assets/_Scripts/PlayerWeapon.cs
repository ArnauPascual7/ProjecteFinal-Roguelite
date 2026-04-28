using System;
using Roguelite.Behaviours;
using UnityEngine;

namespace Roguelite.Player
{
    [RequireComponent(typeof(PlayerInputs))]
    [RequireComponent(typeof(AimRotationBehaviour), typeof(ProjectileFiringBehaviour))]
    public class PlayerWeapon : WeaponController
    {
        private PlayerInputs _playerInputs;

        private AimRotationBehaviour _arb;
        private ProjectileFiringBehaviour _pfb;

        protected override void Awake()
        {
            _arb = GetComponent<AimRotationBehaviour>();
            _pfb = GetComponent<ProjectileFiringBehaviour>();
            _playerInputs = GetComponentInParent<PlayerInputs>();

            base.Awake();

            SwitchWeapon(_playerInputs.SelectedWeapon - 1);
        }

        private void Update()
        {
            AimPosition();
            CheckWeapon();
            CheckFiring();
            CheckReload();
        }

        private void AimPosition()
        {
            Vector2 direction = _arb.GetDirectionTowardsMouse(_playerInputs.MousePosition);
            float angle = _arb.GetAngleTowardsMouse(_playerInputs.MousePosition);

            shootPoint.transform.SetPositionAndRotation((Vector2)transform.position + direction.normalized * 1f, Quaternion.Euler(0f, 0f, angle));
        }

        private void CheckFiring()
        {
            if (_playerInputs.AttackInput)
            {
                Shoot();
            }
        }

        private void CheckReload()
        {
            if (_playerInputs.ReloadInput)
            {
                Reload();
            }
        }

        private void CheckWeapon()
        {
            int index = _playerInputs.SelectedWeapon - 1;
            if (index == CurrentWeaponIndex)
            {
                _playerInputs.SelectedWeapon = CurrentWeaponIndex + 1;
                return;
            }

            SwitchWeapon(index);

            if (CurrentWeaponIndex != index)
            {
                _playerInputs.SelectedWeapon = CurrentWeaponIndex + 1;
            }
        }
    }
}