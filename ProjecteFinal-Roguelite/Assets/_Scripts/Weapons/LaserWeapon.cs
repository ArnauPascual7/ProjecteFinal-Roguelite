using UnityEngine;

namespace Roguelite.Weapons
{
    [CreateAssetMenu(fileName = "LaserWeapon", menuName = "Weapons/Laser Weapon")]
    public class LaserWeapon : RangedWeapon
    {
        [Header("Laser Weapon Stats")]
        public float chargeTime;
        public float chargeDamage;

        public override void Shoot()
        {
            throw new System.NotImplementedException();
        }
    }
}
