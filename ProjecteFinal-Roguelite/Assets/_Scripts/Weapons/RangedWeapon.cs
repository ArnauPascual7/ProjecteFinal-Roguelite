using UnityEngine;

namespace Roguelite.Weapons
{
    public abstract class RangedWeapon : ScriptableObject
    {
        [Header("Weapon Stats")]
        public string weaponName;
        public float damage;
        public float range;
        public float force;

        public abstract void Shoot();
    }
}
