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
    public int SelectedWeapon { get; private set; }

    private float _scroll;
    private const int FIRST_WEAPON = 1;
    private const int SECOND_WEAPON = 2;

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

    public void OnFirst(InputAction.CallbackContext context)
    {
        SelectedWeapon = FIRST_WEAPON;
    }

    public void OnSecond(InputAction.CallbackContext context)
    {
        SelectedWeapon = SECOND_WEAPON;
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        _scroll = context.ReadValue<float>();

        if (_scroll > 0)
        {
            SelectedWeapon = FIRST_WEAPON;
        }
        else if (_scroll < 0)
        {
            SelectedWeapon = SECOND_WEAPON;
        }
    }
}
