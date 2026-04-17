using Roguelite.Behaviours;
using Roguelite.Player;
using UnityEngine;
using UnityEngine.Rendering;

namespace Roguelite.Player
{
    [RequireComponent(typeof(AimRotationBehaviour), typeof(ProjectileFiringBehaviour))]
    public class PlayerWeapon : MonoBehaviour
    {
        private AimRotationBehaviour _arb;
        private ProjectileFiringBehaviour _pfb;
        private PlayerInputs _playerInputs;

        public GameObject bulletPrefab;

        public float angle = 0f;
        public Camera pcamera;   
        public Transform firePoint;

        private void Awake()
        {
            _arb = GetComponent<AimRotationBehaviour>();
            _pfb = GetComponent<ProjectileFiringBehaviour>();
            _playerInputs = GetComponentInParent<PlayerInputs>();
        }
        private void Update()
        {
            AimPosition();
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
            CheckFiring();
        }
        private void AimPosition()
        {
            angle = _arb.GetAngleTowardsMouse(pcamera, _playerInputs.MousePosition);
        }
        private void CheckFiring()
        {
            _pfb.FireProjectile(bulletPrefab, firePoint, _playerInputs.AttackInput);
        }
    }
}