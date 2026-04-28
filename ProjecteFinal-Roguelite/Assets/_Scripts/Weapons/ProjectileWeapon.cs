using System.Collections;
using Roguelite.Behaviours;
using Roguelite.Player;
using UnityEngine;

namespace Roguelite.Weapons
{
    [CreateAssetMenu(fileName = "ProjectileWeapon", menuName = "Weapons/Projectile Weapon")]
    public class ProjectileWeapon : RangedWeapon
    {
        [Header("Projectile Weapon Stats")]
        public int magazineSize;
        public float reloadTime;
        public float fireRate;
        public float projectileSpeed;
        public int projectilesPerShot;
        public float timeBetweenProjectiles;

        [Header("Projectile Prefab")]
        public GameObject projectilePrefab;

        public override RangedWeaponRuntimeState CreateRuntimeState()
            => new ProjectileWeaponRuntimeState { 
                currentMagazine = magazineSize,
                lastFireTime = -fireRate
            };

        public override void Shoot(WeaponController controller, RangedWeaponRuntimeState baseState)
        {
            var state = (ProjectileWeaponRuntimeState)baseState;

            if (state.reloading) return;

            if (state.currentMagazine <= 0)
            {
                Reload(controller, state);
                return;
            }

            if (Time.time <= state.lastFireTime + fireRate) return;

            if (!controller.TryGetComponent(out ProjectileFiringBehaviour fb))
            {
                Debug.LogError($"PROJECTILE WEAPON '{weaponName}': Missing ProjectileFiringBehaviour on WeaponController.");
                return;
            }

            fb.FireProjectile(this, projectilePrefab, controller.shootPoint);
            state.currentMagazine -= projectilesPerShot;
            state.lastFireTime = Time.time;
        }

        public void Reload(WeaponController controller, ProjectileWeaponRuntimeState state)
        {
            if (!state.reloading)
            {
                state.reloading = true;
                state.reloadCoroutine = controller.StartCoroutine(ReloadCoroutine(state));
            }
        }

        private IEnumerator ReloadCoroutine(ProjectileWeaponRuntimeState state)
        {
            yield return new WaitForSeconds(reloadTime);
            state.currentMagazine = magazineSize;
            state.reloading = false;
            state.reloadCoroutine = null;
        }
    }
}
