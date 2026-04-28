using Roguelite.Enemy;
using UnityEngine;

namespace Roguelite.BehaviourTree
{
    [CreateAssetMenu(fileName = "KnockbackState", menuName = "Behaviour Tree/States/Knockback")]
    public class KnockbackSO : BehaviourState
    {
        public override bool EnterCondition(EnemyController econtroller)
        {
            return econtroller.knockback.check;
        }

        public override bool ExitCondition(EnemyController econtroller)
        {
            return econtroller.die.check || !econtroller.knockback.check;
        }
    }
}
