using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Roguelite.GameMenu
{
    public class MenuUIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject starterPanel;
        [SerializeField] private GameObject initialMenuPanel;
        [SerializeField] private GameObject itemsPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject buildPanel;
        [SerializeField] private GameObject upgradesPanel;

        [Header("First Selection")]
        [SerializeField] private GameObject firstButtonInitialMenu;

        private Coroutine _transitionCoroutine;

        private void Start()
        {
            // Mostrar StarterPanel immediatament
            ShowStarter();
        }

        public void ShowStarter()
        {
            // Cridar sense espera
            //ExecuteSwitch(starterPanel, vcamStarterInitial, null);
            SwitchPanels(starterPanel);
        }

        // Menú inicial
        public void ShowInitialMenu()
        {
            bool skipDelay = starterPanel.activeSelf;

            if (skipDelay)
            {
                ExecuteSwitch(initialMenuPanel, MenuCameraManager.Instance.StartingCameraName, firstButtonInitialMenu);
            }    
            else
            {
                StartTransition(initialMenuPanel, MenuCameraManager.Instance.StartingCameraName, firstButtonInitialMenu);
            }           
        }

        // Build
        public void ShowBuild() => StartTransition(buildPanel, VirtualCameraName.Build, null);

        // Millores
        public void ShowUpgrades() => StartTransition(upgradesPanel, VirtualCameraName.Upgrades, null);

        // Items
        public void ShowItems() => StartTransition(itemsPanel, VirtualCameraName.Items, null);

        // Configuració
        public void ShowSettings() => StartTransition(settingsPanel, VirtualCameraName.Settings, null);

        // Lògica de transició
        private void StartTransition(GameObject targetPanel, VirtualCameraName targetCamera, GameObject firstSelect)
        {
            // Si hi ha una transició en marxa, aturar per evitar errors
            if (_transitionCoroutine != null)
            {
                StopCoroutine(_transitionCoroutine);
            }

            _transitionCoroutine = StartCoroutine(TransitionRoutine(targetPanel, targetCamera, firstSelect));
        }

        private IEnumerator TransitionRoutine(GameObject targetPanel, VirtualCameraName targetCamera, GameObject firstSelect)
        {
            // Amagar tots els panells
            HideAllPanels();

            // Iniciar el moviment de la càmera
            MenuCameraManager.Instance.ActivateCamera(targetCamera);

            // Esperar mentre la càmera es mou
            yield return new WaitForSeconds(MenuCameraManager.Instance.DefaultBlend);

            // Mostrar panell i donar focus
            ExecuteSwitch(targetPanel, targetCamera, firstSelect);
        }

        private void ExecuteSwitch(GameObject targetPanel, VirtualCameraName targetCamera, GameObject firstSelect)
        {
            // Activar CameraManager
            MenuCameraManager.Instance.ActivateCamera(targetCamera);

            // Control de visibilitat
            SwitchPanels(targetPanel);

            // Focus del teclat
            if (firstSelect != null)
            {
                EventSystem.current.SetSelectedGameObject(firstSelect);
            }
        }

        private void SwitchPanels(GameObject targetPanel)
        {
            starterPanel.SetActive(starterPanel == targetPanel);
            initialMenuPanel.SetActive(initialMenuPanel == targetPanel);
            itemsPanel.SetActive(itemsPanel == targetPanel);
            settingsPanel.SetActive(settingsPanel == targetPanel);
            buildPanel.SetActive(buildPanel == targetPanel);
            upgradesPanel.SetActive(upgradesPanel == targetPanel);
        }

        private void HideAllPanels()
        {
            starterPanel.SetActive(false);
            initialMenuPanel.SetActive(false);
            itemsPanel.SetActive(false);
            settingsPanel.SetActive(false);
            buildPanel.SetActive(false);
            upgradesPanel.SetActive(false);
        }
    }
}