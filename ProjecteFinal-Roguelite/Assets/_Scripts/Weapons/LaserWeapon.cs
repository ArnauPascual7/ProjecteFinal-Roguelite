using Roguelite.Player;
using UnityEngine;

namespace Roguelite.Weapons
{
    [CreateAssetMenu(fileName = "LaserWeapon", menuName = "Weapons/Laser Weapon")]
    public class LaserWeapon : RangedWeapon
    {
        [Header("Laser Weapon Stats")]
        public float chargeTime;
        public float chargeDamage;

        public override RangedWeaponRuntimeState CreateRuntimeState() => new LaserWeaponRuntimeState();

        public override bool Shoot(WeaponController controller, RangedWeaponRuntimeState baseState)
        {
            var state = (LaserWeaponRuntimeState)baseState;

            throw new System.NotImplementedException();
        }
    }
}
