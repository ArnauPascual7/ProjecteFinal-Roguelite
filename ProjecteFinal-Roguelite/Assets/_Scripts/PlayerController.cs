using Roguelite.Behaviours;
using UnityEngine;

namespace Roguelite.Player
{
    [RequireComponent(typeof(PlayerInputs))]
    [RequireComponent(typeof(MoveBehaviour), typeof(DashBehaviour))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerInputs _playerInputs;
        private MoveBehaviour _mb;
        private DashBehaviour _db;

        private Vector2 _lastMoveDirection = Vector2.right;

        private void Awake()
        {
            _playerInputs = GetComponent<PlayerInputs>();
            _mb = GetComponent<MoveBehaviour>();
            _db = GetComponent<DashBehaviour>();
        }

        private void Update()
        {
            HandleMovement();
            Dash();
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
                Debug.Log("DashInput detectado, dirección: " + _lastMoveDirection);
                _db.OnDash(_lastMoveDirection);
            }
        }
    }
}