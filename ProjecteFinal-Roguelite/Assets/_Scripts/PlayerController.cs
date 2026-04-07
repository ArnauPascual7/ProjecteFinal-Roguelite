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
            _mb.MoveCharacter(_playerInputs.MoveInput.normalized);
        }
        private void Dash()
        {
            if (_playerInputs.DashInput)
            {
                _db.OnDash(_playerInputs.MoveInput.normalized);
            }
        }
    }
}
