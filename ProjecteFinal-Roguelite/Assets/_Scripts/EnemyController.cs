using System.Collections;
using Roguelite.Behaviours;
using Roguelite.BehaviourTree;
using UnityEngine;

namespace Roguelite.Enemy
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(EnemyHealth))]
    [RequireComponent(typeof(TargetDetectionBehaviour), typeof(KnockbackBehaviour))]
    public class EnemyController : MonoBehaviour
    {
        [HideInInspector] public Condition idle;
        [HideInInspector] public Condition chase;
        [HideInInspector] public Condition attack;
        [HideInInspector] public Condition knockback;
        [HideInInspector] public Condition die;

        public ParentStateSO root;
        [Tooltip("This field is initialized via code")]
        public BehaviourState currentState;

        private EnemyHealth _health;

        private TargetDetectionBehaviour _tdb;
        private KnockbackBehaviour _kb;
        private ReturnToInitPosBehaviour _ripb;
        private ChaseBehaviour _cb;
        private MeleeAttackBehaviour _mab;

        private BoxCollider2D _collider;

        private void OnEnable()
        {
            _health.OnEnemyDeath += UpdateDieCheck;

            _tdb.OnTargetDetected += UpdateChaseStateCheck;
            _mab.OnCanAttack += UpdateAttackStateCheck;
            _kb.OnReceiveKnockback += UpdateKnockbackCheck;
        }

        private void OnDisable()
        {
            _health.OnEnemyDeath -= UpdateDieCheck;

            _tdb.OnTargetDetected -= UpdateChaseStateCheck;
            _mab.OnCanAttack -= UpdateAttackStateCheck;
            _kb.OnReceiveKnockback -= UpdateKnockbackCheck;
        }

        private void Awake()
        {
            idle = new Condition("Idle");
            chase = new Condition("Chase");
            attack = new Condition("Attack");
            knockback = new Condition("KnockBack");
            die = new Condition("Die");

            _health = GetComponent<EnemyHealth>();
            _tdb = GetComponent<TargetDetectionBehaviour>();
            _ripb = GetComponent<ReturnToInitPosBehaviour>();
            _cb = GetComponent<ChaseBehaviour>();
            _mab = GetComponent<MeleeAttackBehaviour>();
            _kb = GetComponent<KnockbackBehaviour>();

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
        private void UpdateAttackStateCheck(bool check) => attack.check = check;
        private void UpdateKnockbackCheck(bool check) => knockback.check = check;
        private void UpdateDieCheck(bool check) => die.check = check;

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

        public void IdleStart()
        {
            _cb.StopChase();
        }

        public void ChaseUpdate()
        {
            if (_cb != null)
            {
                _cb.ChaseTarget(_tdb.target.transform);
            }
        }

        public void AttackUpdate()
        {
            if (_mab != null)
            {
                _mab.Attack();
            }

        }

        public void DieStart()
        {
            gameObject.SetActive(false);
        }
    }
}