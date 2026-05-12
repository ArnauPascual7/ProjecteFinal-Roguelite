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
            _states = GetComponent<EnemyAnimState>();

            foreach (var anim in GetComponentsInChildren<Animator>())
            {
                if (anim != _animator)
                {
                    _weaponAnimator = anim;
                    break;
                }
            }

            _tdb = GetComponent<TargetDetectionBehaviour>();
        }

        private void Update()
        {
            UpdateEnemyAnimation();
        }

        private void UpdateEnemyAnimation()
        {
            if (_states.CurrentEnemyState == EnemyStates.Walk || _states.CurrentEnemyState == EnemyStates.Attack)
            {
                // Update both the main animator and the weapon animator with the same direction parameters
                Vector2 direction = DistanceUtils.GetDirection(_tdb.target.transform.position, transform.position);

                _animator.SetFloat(_horizontalParameterName, -direction.x);
                _animator.SetFloat(_verticalParameterName, -direction.y);

                if (_weaponAnimator != null)
                {
                    _weaponAnimator.SetFloat(_horizontalParameterName, -direction.x);
                    _weaponAnimator.SetFloat(_verticalParameterName, -direction.y);
                }

                if (_states.CurrentEnemyState == EnemyStates.Walk)
                {
                    _animator.SetFloat(_velocityParameterName, 1f);
                }
                else
                {
                    _animator.SetFloat(_velocityParameterName, 0f);
                }
            }
            else
            {
                _animator.SetFloat(_horizontalParameterName, 0f);
                _animator.SetFloat(_verticalParameterName, 0f);

                if (_weaponAnimator != null)
                {
                    _weaponAnimator.SetFloat(_horizontalParameterName, 0f);
                    _weaponAnimator.SetFloat(_verticalParameterName, 0f);
                }

                _animator.SetFloat(_velocityParameterName, 0f);
            }

            if (_weaponAnimator != null)
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
}
