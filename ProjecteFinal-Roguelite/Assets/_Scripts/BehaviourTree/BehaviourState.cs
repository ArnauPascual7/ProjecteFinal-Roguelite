using UnityEngine;

namespace Roguelite.BehaviourTree
{
    public abstract class BehaviourState : ScriptableObject
    {
        public abstract bool EnterCondition(EnemyController econtroller);
        public abstract bool ExitCondition(EnemyController econtroller);
        public virtual void OnStart(EnemyController econtroller) { }
        public virtual void OnExit(EnemyController econtroller) { }

        public virtual void OnUpdate(EnemyController econtroller)
        {
            if (ExitCondition(econtroller))
            {
                econtroller.ChangeState();
            }
        }
    }
}
