using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Hellcore.UI.Input
{
    public class InputInteractionBridge : MonoBehaviour
    {
        [Header("Input Settings")]
        [SerializeField] private InputActionReference submitAction;

        [Header("Events")]
        public UnityEvent OnSubmitTriggered;

        public void OnEnable()
        {
            // subscrivim a l'event 'performed' (quan es prem l'acció)
            submitAction.action.performed += OnSubmitInternal;
            submitAction.action.Enable();
        }

        private void OnDisable()
        {
            // desubscriure's
            submitAction.action.performed -= OnSubmitInternal;
            submitAction.action.Disable();
        }

        private void OnSubmitInternal(InputAction.CallbackContext context)
        {
            // L'input ens avisa i nosaltres avisem a qui vulgui escoltar l'event
            OnSubmitTriggered?.Invoke();
        }
    }
}