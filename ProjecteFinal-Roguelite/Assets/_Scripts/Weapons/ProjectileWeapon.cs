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
        public string projectileLayerName;

        public override RangedWeaponRuntimeState CreateRuntimeState()
            => new ProjectileWeaponRuntimeState { 
                currentMagazine = magazineSize,
                lastFireTime = -fireRate
            };

        public override bool Shoot(WeaponController controller, RangedWeaponRuntimeState baseState)
        {
            var state = (ProjectileWeaponRuntimeState)baseState;

            if (state.reloading) return false;

            if (state.currentMagazine <= 0)
            {
                Reload(controller, state);
                return false;
            }

            float effectiveFireRate = fireRate / controller.AttackSpeedMultiplier;

            if (Time.time <= state.lastFireTime + effectiveFireRate) return false;

            if (!controller.TryGetComponent(out ProjectileFiringBehaviour fb))
            {
                Debug.LogError($"PROJECTILE WEAPON '{weaponName}': Missing ProjectileFiringBehaviour on WeaponController.");
                return false;
            }
            
            fb.FireProjectile(this, projectilePrefab, controller.shootPoint);
            state.currentMagazine -= projectilesPerShot;
            state.lastFireTime = Time.time;
            return true;
        }

        public void Reload(WeaponController controller, ProjectileWeaponRuntimeState state)
        {
            if (!state.reloading)
            {
                state.reloading = true;
                state.reloadCoroutine = controller.StartCoroutine(ReloadCoroutine(state, controller));
            }
        }

        private IEnumerator ReloadCoroutine(ProjectileWeaponRuntimeState state, WeaponController controller)
        {
            // Reduir temps d'espera segons millora
            float effectiveReloadTime = reloadTime / controller.ReloadSpeedMultiplier;

            yield return new WaitForSeconds(effectiveReloadTime);

            state.currentMagazine = magazineSize;
            state.reloading = false;
            state.reloadCoroutine = null;
        }
    }
}
