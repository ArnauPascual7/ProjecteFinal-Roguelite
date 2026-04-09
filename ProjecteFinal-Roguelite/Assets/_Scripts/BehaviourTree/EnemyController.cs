using System.Collections;
using Roguelite.Behaviours;
using UnityEngine;

namespace Roguelite.BehaviourTree
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(TargetDetectionBehaviour))]
    public class EnemyController : MonoBehaviour
    {
        [HideInInspector] public Condition idle;
        [HideInInspector] public Condition chase;
        [HideInInspector] public Condition attack;
        [HideInInspector] public Condition die;

        public ParentStateSO root;
        [Tooltip("This field is initialized via code")]
        public BehaviourState currentState;

        private TargetDetectionBehaviour _tdb;

        private void Awake()
        {
            idle = new Condition("Idle");
            chase = new Condition("Chase");
            attack = new Condition("Attack");
            die = new Condition("Die");

            _tdb = new TargetDetectionBehaviour();
        }

        private void Start()
        {
            ChangeState();
        }

        private void Update()
        {
            if (currentState != null)
            {
                currentState.OnUpdate(this);
            }

            if (_tdb.IsDetected)
            {
                chase.check = true;
            }
            else
            {
                chase.check = false;
            }
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