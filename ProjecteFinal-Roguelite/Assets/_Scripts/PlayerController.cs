using System;
using Roguelite.Behaviours;
using UnityEngine;

namespace Roguelite.Player
{
    [RequireComponent(typeof(PlayerInputs), typeof(PlayerAnimation), typeof(PlayerState))]
    [RequireComponent(typeof(MoveBehaviour), typeof(DashBehaviour), typeof(StaminaBehaviour))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerInputs _playerInputs;
        private PlayerState _playerState;

        private MoveBehaviour _mb;
        private DashBehaviour _db;
        private StaminaBehaviour _sb;

        private void Awake()
        {
            _playerInputs = GetComponent<PlayerInputs>();
            _playerState = GetComponent<PlayerState>();

            _mb = GetComponent<MoveBehaviour>();
            _db = GetComponent<DashBehaviour>();
            _sb = GetComponent<StaminaBehaviour>();
        }

        private void Update()
        {
            UpdateState();

            HandleMovement();
            Dash();

            StaminaRegeneration();
        }

        private void UpdateState()
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

        private void HandleMovement()
        {
            if (_db.IsDashing) return; 

            _mb.MoveCharacter(_playerInputs.MoveInput.normalized);
        }

        private void Dash()
        {
            if (_playerInputs.MoveInput == Vector2.zero) return;

            if (_playerInputs.DashInput)
            {
                if (_sb.HasStamina() && _db.CanDash)
                {
                    _db.Dash(_lastMoveDirection);
                    _sb.ConsumeStamina(_db.DashCooldown);
                }
            }
        }
        
        private void StaminaRegeneration()
        {
            if (!_db.IsDashing)
            {
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
    }
}