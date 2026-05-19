using Roguelite.Behaviours;
using Roguelite.Enemy;
using UnityEngine;

namespace Roguelite.BehaviourTree
{
    [CreateAssetMenu(fileName = "IdleState", menuName = "Behaviour Tree/States/Idle")]
    public class IdleStateSO : BehaviourState
    {
        public override bool EnterCondition(EnemyController econtroller)
        {
            return true;
        }

        public override bool ExitCondition(EnemyController econtroller)
        {
            return econtroller.chase.check || econtroller.attack.check || econtroller.knockback.check || econtroller.die.check;
        }

        public override void OnStart(EnemyController econtroller)
        {
            econtroller.IdleStart();
        }

        public override void OnExit(EnemyController econtroller)
        {
            base.OnExit(econtroller);

            if (econtroller.TryGetComponent<ChaseBehaviour>(out var cb)) cb.ResumeChasing();
        }
    }
}
