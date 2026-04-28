using UnityEngine;
using UnityEngine.InputSystem;

namespace Roguelite.GameMenu
{
    public class MenuInputs : MonoBehaviour, InputSystem_Actions.IMenuActions
    {
        public InputSystem_Actions InputActions { get; private set; }

        public bool AnyKeyInput { get; private set; }

        private void OnEnable()
        {
            InputActions = new InputSystem_Actions();
            InputActions.Menu.Enable();
            InputActions.Menu.AddCallbacks(this);
        }

        private void OnDisable()
        {
            InputActions.Menu.Disable();
            InputActions.Menu.RemoveCallbacks(this);
        }

        public void OnAnyKey(InputAction.CallbackContext context)
        {
            AnyKeyInput = context.ReadValueAsButton();
        }
    }
}