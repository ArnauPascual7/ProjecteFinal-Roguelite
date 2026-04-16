using UnityEngine;

namespace Roguelite.Player
{
    public class PlayerState : MonoBehaviour
    {
        public PlayerStates CurrentPlayerState { get; set; } = PlayerStates.Idle;

        public bool InMoveState()
        {
            return  CurrentPlayerState == PlayerStates.Walk || 
                    CurrentPlayerState == PlayerStates.Dash;
        }
    }

    public enum PlayerStates
    {
        Idle,
        Walk,
        Dash,
        Dead
    }
}
