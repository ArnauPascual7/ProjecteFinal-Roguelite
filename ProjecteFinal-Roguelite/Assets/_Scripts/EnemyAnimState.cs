using UnityEngine;

namespace Roguelite.Enemy
{
    public class EnemyAnimState : MonoBehaviour
    {
        public EnemyStates CurrentEnemyState { get; set; } = EnemyStates.Idle;
        public EnemyWeaponStates CurrentEnemyWeaponState { get; set; } = EnemyWeaponStates.Idle;
    }

    public enum EnemyStates
    {
        Idle,
        Walk,
        Attack,
        Dead
    }

    public enum EnemyWeaponStates
    {
        Idle,
        Attack
    }
}
