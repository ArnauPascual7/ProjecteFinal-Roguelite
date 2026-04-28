using Roguelite.Enemy;
using UnityEngine;

namespace Roguelite.BehaviourTree
{
    [CreateAssetMenu(fileName = "ComeBackState", menuName = "Behaviour Tree/States/ComeBack")]
    public class ComeBackStateSO : BehaviourState
    {
        public override bool EnterCondition(EnemyController econtroller)
        {
            return econtroller.comeBack.check;
        }

        public override bool ExitCondition(EnemyController econtroller)
        {
            return econtroller.chase.check || econtroller.attack.check || econtroller.die.check || !econtroller.comeBack.check;
        }

        public override void OnUpdate(EnemyController econtroller)
        {
            base.OnUpdate(econtroller);

            econtroller.ComeBack();
        }
    }
}
