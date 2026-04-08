using System.Collections;
using UnityEngine;

namespace Roguelite.BehaviourTree
{
    public class EnemyController : MonoBehaviour
    {
        [HideInInspector] public Condition idle;
        [HideInInspector] public Condition chase;
        [HideInInspector] public Condition attack;
        [HideInInspector] public Condition die;

        public ParentStateSO root;
        public BehaviourState currentState;

        private void Awake()
        {
            idle = new Condition("Idle");
            chase = new Condition("Chase");
            attack = new Condition("Attack");
            die = new Condition("Die");
        }

        private void Start()
        {
            ChangeState();
        }

        public void ChangeState()
        {
            StartCoroutine(WaitToTheEndOfFrame());
        }

        private IEnumerator WaitToTheEndOfFrame()
        {
            yield return new WaitForEndOfFrame();

            foreach (var node in root.states)
            {
                if (node.EnterCondition(this))
                {
                    if (currentState != null)
                    {
                        currentState.OnExit(this);
                    }

                    currentState = node;
                    node.OnStart(this);

                    break;
                }
            }
        }
    }
}