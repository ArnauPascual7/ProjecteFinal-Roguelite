using System.Collections;
using Roguelite.Behaviours;
using Roguelite.BehaviourTree;
using UnityEngine;

namespace Roguelite.Enemy
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimation), typeof(EnemyAnimState))]
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
        private EnemyAnimation _animator;
        private EnemyAnimState _animStates;
        private EnemyWeapon _weapon;

        private TargetDetectionBehaviour _tdb;
        private KnockbackBehaviour _kb;
        private ReturnToInitPosBehaviour _ripb;
        private ChaseBehaviour _cb;
        private MoveBehaviour _mb;
        private MeleeAttackBehaviour _mab;
        private ProjectileFiringBehaviour _pfb;

        private BoxCollider2D _collider;

        private void OnEnable()
        {
            _health.OnEnemyDeath += UpdateDieCheck;
            
            _tdb.OnTargetDetected += UpdateChaseStateCheck;
            if (_mab != null) _mab.OnCanAttack += UpdateAttackStateCheck;
            if (_kb != null) _kb.OnReceiveKnockback += UpdateKnockbackCheck;
        }

        private void OnDisable()
        {
            _health.OnEnemyDeath -= UpdateDieCheck;
            
            _tdb.OnTargetDetected -= UpdateChaseStateCheck;
            if (_mab != null) _mab.OnCanAttack -= UpdateAttackStateCheck;
            if (_kb != null) _kb.OnReceiveKnockback -= UpdateKnockbackCheck;
        }

        private void Awake()
        {
            idle = new Condition("Idle");
            chase = new Condition("Chase");
            attack = new Condition("Attack");
            knockback = new Condition("KnockBack");
            die = new Condition("Die");

            InitializeComponents();

            _collider = GetComponent<BoxCollider2D>();
            _collider.isTrigger = false;
        }

        public void InitializeEnemy(EnemyData data)
        {
            InitializeComponents();

            _tdb.target = data.target;
            _mb.speed = data.speed;
        }

        private void InitializeComponents()
        {
            _health = GetComponent<EnemyHealth>();
            _animator = GetComponent<EnemyAnimation>();
            _animStates = GetComponent<EnemyAnimState>();
            _weapon = GetComponent<EnemyWeapon>();

            _tdb = GetComponent<TargetDetectionBehaviour>();
            _ripb = GetComponent<ReturnToInitPosBehaviour>();
            _cb = GetComponent<ChaseBehaviour>();
            _mb = GetComponent<MoveBehaviour>();
            _mab = GetComponent<MeleeAttackBehaviour>();
            _kb = GetComponent<KnockbackBehaviour>();
            _pfb = GetComponent<ProjectileFiringBehaviour>();
        }

        public void InitializeEnemy(EnemyData data)
        {
            _tdb.target = data.target;
            _mb.Speed = data.speed;
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

            _animStates.CurrentEnemyState = EnemyStates.Idle;
            _animStates.CurrentEnemyWeaponState = EnemyWeaponStates.Idle;

            if (_weapon != null)
            {
                _weapon.ResetPosition();
            }
        }

        public void ChaseUpdate()
        {
            if (_cb != null)
            {
                _cb.ChaseTarget(_tdb.target.transform);
                if (_weapon != null)
                {
                    _weapon.AimPosition();
                    _weapon.Shoot();

                    if (_mab == null)
                    {
                        _animStates.CurrentEnemyWeaponState = EnemyWeaponStates.Attack;
                    }
                }

                _animStates.CurrentEnemyState = EnemyStates.Walk;
            }

            if (_mab != null)
            {
                _animStates.CurrentEnemyWeaponState = EnemyWeaponStates.Idle;
            }
        }

        public void AttackUpdate()
        {
            if (_mab != null)
            {
                _mab.Attack();

                _animStates.CurrentEnemyState = EnemyStates.Attack;
                _animStates.CurrentEnemyWeaponState = EnemyWeaponStates.Attack;
            }
        }

        public void DieStart()
        {
            _animStates.CurrentEnemyState = EnemyStates.Dead;
            _animStates.CurrentEnemyWeaponState = EnemyWeaponStates.Idle;

            gameObject.SetActive(false);
        }
    }
}