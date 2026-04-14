using UnityEngine;

namespace Roguelite.UI
{
    public class StarterToMainBridge : MonoBehaviour
    {
        [SerializeField] private MenuInputs menuInputs;
        [SerializeField] private MenuUIManager uiManager;

        private bool _hasTransitioned = false;

        private void Update()
        {
            // Si estem a StarterPanel i l'usuari prem qualsevol tecla
            if (!_hasTransitioned && menuInputs.AnyKeyInput)
            {
                _hasTransitioned = true; // Evitar que es cridi mes d'una vegada
                uiManager.ShowInitialMenu();
            }
        }
    }
}
