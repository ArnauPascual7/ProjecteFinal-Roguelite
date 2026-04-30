using Roguelite.Enemy;
using UnityEngine;

namespace Roguelite.BehaviourTree
{
    [CreateAssetMenu(fileName = "ChaseState", menuName = "Behaviour Tree/States/Chase")]
    public class ChaseStateSO : BehaviourState
    {
        public override bool EnterCondition(EnemyController econtroller)
        {
            return econtroller.chase.check;
        }

        public override bool ExitCondition(EnemyController econtroller)
        {
            return econtroller.attack.check || econtroller.knockback.check || econtroller.die.check || !econtroller.chase.check;
        }

        public override void OnUpdate(EnemyController econtroller)
        {
            base.OnUpdate(econtroller);

            econtroller.ChaseUpdate();
        }
    }
}
