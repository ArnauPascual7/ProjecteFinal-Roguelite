using Roguelite.Behaviours;
using Roguelite.Helpers;
using UnityEngine;

namespace Roguelite.Enemy
{
    [RequireComponent(typeof(Animator), typeof(EnemyAnimState))]
    public class EnemyAnimation : MonoBehaviour
    {
        [Header("Animator Parameter Names")]
        [SerializeField] private string _horizontalParameterName = "X";
        [SerializeField] private string _verticalParameterName = "Y";
        [SerializeField] private string _velocityParameterName = "Velocity";

        [Header("Weapon Animator Parameter Names")]
        [SerializeField] private string _weaponAttackParameterName = "Attack";

        private Animator _animator;
        private Animator _weaponAnimator;
        private EnemyAnimState _states;

        private TargetDetectionBehaviour _tdb;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _weaponAnimator = GetComponentInChildren<Animator>();
            _states = GetComponent<EnemyAnimState>();

            _tdb = GetComponent<TargetDetectionBehaviour>();
        }

        private void Update()
        {
            UpdateEnemyAnimation();
            UpdateEnemyWeaponAnimation();
        }

        private void UpdateEnemyAnimation()
        {
            if (_states.CurrentEnemyState == EnemyStates.Walk)
            {
                _animator.SetFloat(_horizontalParameterName, -DistanceUtils.GetDirection(_tdb.target.transform.position, transform.position).x);
                _animator.SetFloat(_verticalParameterName, -DistanceUtils.GetDirection(_tdb.target.transform.position, transform.position).y);
                _animator.SetFloat(_velocityParameterName, 1f);
            }
            else
            {
                _animator.SetFloat(_horizontalParameterName, 0f);
                _animator.SetFloat(_verticalParameterName, 0f);
                _animator.SetFloat(_velocityParameterName, 0f);
            }
        }

        private void UpdateEnemyWeaponAnimation()
        {
            if (_states.CurrentEnemyWeaponState == EnemyWeaponStates.Attack)
            {
                _weaponAnimator.SetBool(_weaponAttackParameterName, true);
            }
            else
            {
                _weaponAnimator.SetBool(_weaponAttackParameterName, false);
            }
        }
    }
}
