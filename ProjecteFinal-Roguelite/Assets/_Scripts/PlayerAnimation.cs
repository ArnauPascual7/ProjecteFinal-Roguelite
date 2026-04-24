using UnityEngine;

namespace Roguelite.Player
{
    [RequireComponent(typeof(Animator), typeof(PlayerInputs), typeof(PlayerState))]
    public class PlayerAnimation : MonoBehaviour
    {
        [Header("Animator Parameter Names")]
        [SerializeField] private string _horizontalParameterName = "X";
        [SerializeField] private string _verticalParameterName = "Y";
        [SerializeField] private string _velocityParameterName = "Velocity";
        [SerializeField] private string _isDashingParameterName = "IsDashing";
        [SerializeField] private string _isDeadParameterName = "IsDead";

        private Animator _animator;
        private PlayerInputs _inputs;
        private PlayerState _states;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _inputs = GetComponent<PlayerInputs>();
            _states = GetComponent<PlayerState>();
        }

        private void Update()
        {
            if (_states.InMoveState())
            {
                _animator.SetFloat(_horizontalParameterName, _inputs.MoveInput.x);
                _animator.SetFloat(_verticalParameterName, _inputs.MoveInput.y);
            }

            _animator.SetFloat(_velocityParameterName, _inputs.MoveInput.magnitude);
            _animator.SetBool(_isDashingParameterName, _states.CurrentPlayerState == PlayerStates.Dash);
            _animator.SetBool(_isDeadParameterName, _states.CurrentPlayerState == PlayerStates.Dead);
        }
    }
}
