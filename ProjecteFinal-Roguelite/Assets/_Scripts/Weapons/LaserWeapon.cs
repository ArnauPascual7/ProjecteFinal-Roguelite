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

        public override RangedWeaponRuntimeState CreateRuntimeState() => new LaserRuntimeState();

        public override void Shoot(WeaponController controller, RangedWeaponRuntimeState baseState)
        {
            var state = (LaserRuntimeState)baseState;

            throw new System.NotImplementedException();
        }
    }
}
