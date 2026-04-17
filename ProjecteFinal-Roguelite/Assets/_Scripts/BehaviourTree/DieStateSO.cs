using Roguelite.Enemy;
using UnityEngine;

namespace Roguelite.BehaviourTree
{
    [CreateAssetMenu(fileName = "DieState", menuName = "Behaviour Tree/States/Die")]
    public class DieStateSO : BehaviourState
    {
        public override bool EnterCondition(EnemyController econtroller)
        {
            return econtroller.die.check;
        }

        public override bool ExitCondition(EnemyController econtroller)
        {
            return false;
        }

        public override void OnStart(EnemyController econtroller)
        {
            econtroller.Die();
        }
    }
}
