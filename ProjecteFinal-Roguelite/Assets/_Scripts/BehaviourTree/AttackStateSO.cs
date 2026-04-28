using Roguelite.Enemy;
using UnityEngine;

namespace Roguelite.BehaviourTree
{
    [CreateAssetMenu(fileName = "AttackState", menuName = "Behaviour Tree/States/Attack")]
    public class AttackStateSO : BehaviourState
    {
        public override bool EnterCondition(EnemyController econtroller)
        {
            return econtroller.attack.check;
        }

        public override bool ExitCondition(EnemyController econtroller)
        {
            return econtroller.die.check || econtroller.knockback.check || !econtroller.attack.check;
        }

        public override void OnUpdate(EnemyController econtroller)
        {
            base.OnUpdate(econtroller);

            econtroller.Attack();
        }
    }
}
