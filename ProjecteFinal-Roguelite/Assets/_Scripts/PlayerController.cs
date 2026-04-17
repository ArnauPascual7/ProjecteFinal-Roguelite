using Roguelite.Behaviours;
using UnityEngine;

namespace Roguelite.Player
{
    [RequireComponent(typeof(PlayerInputs), typeof(PlayerHealth))]
    [RequireComponent(typeof(MoveBehaviour), typeof(DashBehaviour), typeof(StaminaBehaviour))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerInputs _playerInputs;
        private MoveBehaviour _mb;
        private DashBehaviour _db;
        private StaminaBehaviour _sb;

        private Vector2 _lastMoveDirection = Vector2.right;

        private void Awake()
        {
            _playerInputs = GetComponent<PlayerInputs>();
            _mb = GetComponent<MoveBehaviour>();
            _db = GetComponent<DashBehaviour>();
            _sb = GetComponent<StaminaBehaviour>();
        }

        private void Update()
        {
            HandleMovement();
            Dash();

            StaminaRegeneration();
        }

        private void HandleMovement()
        {
            if (_db.IsDashing) return; 

            _mb.MoveCharacter(_playerInputs.MoveInput.normalized);

            if (_playerInputs.MoveInput != Vector2.zero)
                _lastMoveDirection = _playerInputs.MoveInput.normalized;
        }

        private void Dash()
        {
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