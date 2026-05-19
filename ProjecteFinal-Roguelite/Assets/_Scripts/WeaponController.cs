using System;
using Roguelite.Weapons;
using UnityEngine;

namespace Roguelite
{
    public abstract class WeaponController : MonoBehaviour
    {
        [SerializeField] protected RangedWeapon[] _weapons;
        [SerializeField] private float _switchCooldown = 1f;

        public Transform shootPoint;
        public int CurrentWeaponIndex { get; private set; }
        public float DamageMultiplier { get; private set; } = 1f;
        public float AttackSpeedMultiplier { get; private set; } = 1f;
        public float ProjectileSpeedMultiplier { get; private set; } = 1f;
        public float ReloadSpeedMultiplier { get; private set; } = 1f;

        private RangedWeapon _currentWeapon;
        private RangedWeaponRuntimeState[] _weaponStates;
        private RangedWeaponRuntimeState _currentState;
        private float _lastSwitchTime;

        protected virtual void Awake()
        {
            _weaponStates = new RangedWeaponRuntimeState[_weapons.Length];
            for (int i = 0; i < _weapons.Length; i++)
            {
                if (_weapons[i] != null)
                {
                    _weaponStates[i] = _weapons[i].CreateRuntimeState();
                }
            }

            _currentWeapon = _weapons[0];
            _currentState = _weaponStates[0];
            CurrentWeaponIndex = 0;
        }

        public void SwitchWeapon(int index)
        {
            if (Time.time <= _lastSwitchTime + _switchCooldown) return;

            if (index < 0 || index >= _weapons.Length)
            {
                Debug.LogError("WEAPON CONTROLLER: Invalid weapon index: " + index);
                return;
            }

            _currentWeapon = _weapons[index];
            _currentState = _weaponStates[index];
            CurrentWeaponIndex = index;
            
            _lastSwitchTime = Time.time;
            _currentState.lastFireTime = Time.time;
        }

        public void Shoot()
        {
            if (_currentWeapon == null)
            {
                Debug.LogError("WEAPON CONTROLLER: No weapon assigned to shoot.");
                return;
            }

            _currentWeapon.Shoot(this, _currentState);
        }

        public void Reload()
        {
            if (_currentWeapon == null)
            {
                Debug.LogError("WEAPON CONTROLLER: No weapon assigned to reload.");
                return;
            }

            if (_currentWeapon is ProjectileWeapon pw)
            {
                pw.Reload(this, (ProjectileWeaponRuntimeState)_currentState);
            }
        }

        // Mètodes d'injecció
        public void SetDamageBonus(float percentageIncrease)
        {
            DamageMultiplier = 1f + (percentageIncrease / 100f);
        }

        public void SetAttackSpeedBonus(float percentageIncrease)
        {
            AttackSpeedMultiplier = 1f + (percentageIncrease / 100f);
        }

        public void SetProjectileSpeedBonus(float percentageIncrease)
        {
            ProjectileSpeedMultiplier = 1f + (percentageIncrease / 100f);
        }

        public void SetReloadSpeedBonus(float percentageIncrease)
        {
            ReloadSpeedMultiplier = 1f + (percentageIncrease / 100f);
        }
    }
}