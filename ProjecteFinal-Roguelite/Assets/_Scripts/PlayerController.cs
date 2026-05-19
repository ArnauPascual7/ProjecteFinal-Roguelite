using System;
using System.Collections;
using Roguelite.Behaviours;
using Roguelite.Systems;
using UnityEngine;

namespace Roguelite.Player
{
    [RequireComponent(typeof(PlayerInputs), typeof(PlayerAnimation), typeof(PlayerState))]
    [RequireComponent(typeof(PlayerHealth), typeof(PlayerSounds))]
    [RequireComponent(typeof(MoveBehaviour), typeof(DashBehaviour), typeof(StaminaBehaviour))]
    [RequireComponent(typeof(KnockbackBehaviour), typeof(MagicPointsBehaviour))]
    public class PlayerController : MonoBehaviour
    {
        public static event Action<float> OnHealthChange;
        public static event Action<float> OnStaminaChange;
        public static event Action<float> OnMagicPointsChange;
        public static event Action<float> OnMaxHealthChange;
        public static event Action<float, float> OnStaminaStart;

        private PlayerInputs _playerInputs;
        private PlayerHealth _playerHealth;
        private PlayerState _playerState;
        private PlayerSounds _playerSounds;

        public StaminaBehaviour Stamina => _sb;

        private MoveBehaviour _mb;
        private DashBehaviour _db;
        private StaminaBehaviour _sb;
        private KnockbackBehaviour _kb;
        private MagicPointsBehaviour _mpb;

        private void OnEnable()
        {
            _playerHealth.OnHealthChange += HealthChange;
            _playerHealth.OnMaxHealthChange += MaxHealthChange;
        }

        private void OnDisable()
        {
            _playerHealth.OnHealthChange -= HealthChange;
            _playerHealth.OnMaxHealthChange -= MaxHealthChange;
        }

        private void HealthChange(float h) => OnHealthChange?.Invoke(h);
        private void MaxHealthChange(float mh) => OnMaxHealthChange?.Invoke(mh);

        private void Awake()
        {
            _playerInputs = GetComponent<PlayerInputs>();
            _playerHealth = GetComponent<PlayerHealth>();
            _playerState = GetComponent<PlayerState>();
            _playerSounds = GetComponent<PlayerSounds>();

            _mb = GetComponent<MoveBehaviour>();
            _db = GetComponent<DashBehaviour>();
            _sb = GetComponent<StaminaBehaviour>();
            _kb = GetComponent<KnockbackBehaviour>();
            _mpb = GetComponent<MagicPointsBehaviour>();
            
            StartCoroutine(GMPlayerControllerInitCorroutine());
        }

        private IEnumerator GMPlayerControllerInitCorroutine()
        {
            yield return new WaitUntil(() => GameManager.Instance != null);

            GameManager.Instance.PlayerControllerInit(this);

            OnStaminaStart?.Invoke(_sb._baseMaxStamina, _sb.currentStamina);
        }

        private void Update()
        {
            UpdateState();

            HandleMovement();
            Dash();
            
            StaminaRegeneration();
            MagicPointsRegeneration();

            _playerSounds.PlayPlayerFootsteps(_playerInputs.MoveInput != Vector2.zero, _db.IsDashing);
        }

        private void UpdateState()
        {
            bool isDead = !_playerHealth.IsAlive;

            if (!isDead)
            {
                bool isMoving = _playerInputs.MoveInput != Vector2.zero;
                bool isDashing = _db.IsDashing;

                if (isMoving)
                {
                    _playerState.CurrentPlayerState = isDashing ? PlayerStates.Dash : PlayerStates.Walk;
                }
                else
                {
                    _playerState.CurrentPlayerState = PlayerStates.Idle;
                }
            }
            else
            {
                _playerState.CurrentPlayerState = PlayerStates.Dead;
            }
        }

        private void HandleMovement()
        {
            if (_kb.IsReceivingKnockback) return;
            if (_db.IsDashing) return; 

            _mb.MoveCharacter(_playerInputs.MoveInput.normalized);
        }

        private void Dash()
        {
            if (_kb.IsReceivingKnockback) return;

            OnStaminaChange?.Invoke(_sb.currentStamina);
            if (_playerInputs.MoveInput == Vector2.zero) return;

            if (_playerInputs.DashInput)
            {
                if (_sb.HasStamina() && _db.CanDash)
                {
                    _db.Dash(_playerInputs.MoveInput);
                    _sb.ConsumeStamina(_db.DashCooldown);
                    _playerSounds.PlayPlayerDash();
                }
            }
        }
        
        private void StaminaRegeneration()
        {
            if (!_db.IsDashing)
            {
                OnStaminaChange?.Invoke(_sb.currentStamina);
                if (_playerInputs.MoveInput != Vector2.zero)
                {
                    _sb.RegenerateStamina(_db.DashCooldown);
                }
                else
                {
                    _sb.RegenerateStamina(_db.DashCooldown, 2f);
                }
            }
        }
        private void MagicPointsRegeneration()
        {
            OnMagicPointsChange?.Invoke(_mpb.currentMagicPoints);
            _mpb.RegenerateMagicPoints();
        }
    }
}