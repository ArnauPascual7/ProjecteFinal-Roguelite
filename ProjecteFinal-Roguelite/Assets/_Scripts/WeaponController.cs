using System;
using Roguelite.Weapons;
using UnityEngine;

namespace Roguelite
{
    public abstract class WeaponController : MonoBehaviour
    {
        [SerializeField] private RangedWeapon[] _weapons;

        public Transform shootPoint;
        public int CurrentWeaponIndex { get; private set; }

        private RangedWeapon _currentWeapon;
        private RangedWeaponRuntimeState[] _weaponStates;
        private RangedWeaponRuntimeState _currentState;

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
            if (index < 0 || index >= _weapons.Length)
            {
                Debug.LogError("WEAPON CONTROLLER: Invalid weapon index: " + index);
                return;
            }

            _currentWeapon = _weapons[index];
            _currentState = _weaponStates[index];
            CurrentWeaponIndex = index;
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
    }
}