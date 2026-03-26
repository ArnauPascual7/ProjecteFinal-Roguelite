using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public InputSystem_Actions InputActions { get; private set; }
    
    public Vector2 MoveInput { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public bool AttackInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool InteractInput { get; private set; }

    private void OnEnable()
    {
        InputActions = new InputSystem_Actions();
        InputActions.Player.Enable();
        InputActions.Player.AddCallbacks(this);
    }

    private void OnDisable()
    {
        InputActions.Player.Disable();
        InputActions.Player.RemoveCallbacks(this);
    }

    private void LateUpdate()
    {
        DashInput = false;
        InteractInput = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        MousePosition = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        AttackInput = context.ReadValueAsButton();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        DashInput = context.ReadValueAsButton();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        InteractInput = context.ReadValueAsButton();
    }
}
