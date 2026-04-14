using System.Collections;
using Roguelite.Behaviours;
using Roguelite.BehaviourTree;
using UnityEngine;

namespace Roguelite.Enemy
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(TargetDetectionBehaviour))]
    public class EnemyController : MonoBehaviour
    {
        [HideInInspector] public Condition idle;
        [HideInInspector] public Condition comeBack;
        [HideInInspector] public Condition chase;
        [HideInInspector] public Condition attack;
        [HideInInspector] public Condition die;

        public ParentStateSO root;
        [Tooltip("This field is initialized via code")]
        public BehaviourState currentState;

        private TargetDetectionBehaviour _tdb;
        private ReturnToInitPosBehaviour _ripb;
        private ChaseBehaviour _cb;
        private MeleeAttackBehaviour _mab;

        private BoxCollider2D _collider;

        private void OnEnable()
        {
            _tdb.OnTargetDetected += UpdateChaseStateCheck;
            _ripb.OnReachDestination += UpdateComeBackStateCheck;
            _mab.OnCanAttack += UpdateAttackStateCheck;
        }

        private void OnDisable()
        {
            _tdb.OnTargetDetected -= UpdateChaseStateCheck;
            _ripb.OnReachDestination -= UpdateComeBackStateCheck;
            _mab.OnCanAttack -= UpdateAttackStateCheck;
        }

        private void Awake()
        {
            idle = new Condition("Idle");
            comeBack = new Condition("CumBack");
            chase = new Condition("Chase");
            attack = new Condition("Attack");
            die = new Condition("Die");

            _tdb = GetComponent<TargetDetectionBehaviour>();
            _ripb = GetComponent<ReturnToInitPosBehaviour>();
            _cb = GetComponent<ChaseBehaviour>();
            _mab = GetComponent<MeleeAttackBehaviour>();

            _collider = GetComponent<BoxCollider2D>();
            _collider.isTrigger = false;
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
        }

        private void UpdateChaseStateCheck(bool check) => chase.check = check;
        private void UpdateComeBackStateCheck(bool check) => comeBack.check = check;
        private void UpdateAttackStateCheck(bool check) => attack.check = check;

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

        public void ComeBack()
        {
            if (_ripb != null)
            {
                _ripb.ReturnToInitialPosition();
            }
        }

        public void Chase()
        {
            if (_cb != null)
            {
                _cb.ChaseTarget(_tdb.target.transform);
            }
        }

        public void Attack()
        {
            if (_mab != null)
            {
                _mab.Attack();
            }
        }

        public void Die()
        {
            gameObject.SetActive(false);
        }
    }
}