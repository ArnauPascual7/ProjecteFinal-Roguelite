using Roguelite.Behaviours;
using UnityEngine;

namespace Roguelite.Player
{
    [RequireComponent(typeof(PlayerInputs))]
    [RequireComponent(typeof(MoveBehaviour))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerInputs _playerInputs;
        private MoveBehaviour _mb;

        private void Awake()
        {
            _playerInputs = GetComponent<PlayerInputs>();
            _mb = GetComponent<MoveBehaviour>();    
        }

        private void Update()
        {
            HandleMovement();
        }

        private void HandleMovement()
        {
            _mb.MoveCharacter(_playerInputs.MoveInput.normalized);
        }
    }
}
