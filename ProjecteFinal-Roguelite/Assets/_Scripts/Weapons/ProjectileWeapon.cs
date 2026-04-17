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
        public float projectilesPerShot;

        public override void Shoot()
        {
            throw new System.NotImplementedException();
        }
    }
}
