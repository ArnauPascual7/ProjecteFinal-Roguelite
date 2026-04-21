using System.Collections;
using Roguelite.camera;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Roguelite.UI
{
    public class MenuUIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MenuCameraManager cameraManager;

        [Header("Panels")]
        [SerializeField] private GameObject starterPanel;
        [SerializeField] private GameObject initialMenuPanel;
        [SerializeField] private GameObject itemsPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject buildPanel;

        [Header("Virtual Cameras")]
        [SerializeField] private CinemachineCamera vcamStarterInitial;
        [SerializeField] private CinemachineCamera vcamItems;
        [SerializeField] private CinemachineCamera vcamSettings;
        [SerializeField] private CinemachineCamera vcamBuild;

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
            ExecuteSwitch(starterPanel, vcamStarterInitial, null);
        }

        // Menú inicial
        public void ShowInitialMenu()
        {
            bool skipDelay = starterPanel.activeSelf;

            if (skipDelay)
            {
                ExecuteSwitch(initialMenuPanel, vcamStarterInitial, firstButtonInitialMenu);
            }    
            else
            {
                StartTransition(initialMenuPanel, vcamStarterInitial, firstButtonInitialMenu);
            }           
        }

        // Items
        public void ShowItems() => StartTransition(itemsPanel, vcamItems, null);

        // Configuració
        public void ShowSettings() => StartTransition(settingsPanel, vcamSettings, null);

        // Build
        public void ShowBuild() => StartTransition(buildPanel, vcamBuild, null);

        // Lògica de transició
        private void StartTransition(GameObject targetPanel, CinemachineCamera targetVcam, GameObject firstSelect)
        {
            // Si hi ha una transició en marxa, aturar per evitar errors
            if (_transitionCoroutine != null)
            {
                StopCoroutine(_transitionCoroutine);
            }
                
            _transitionCoroutine = StartCoroutine(TransitionRoutine(targetPanel, targetVcam, firstSelect));
        }

        private IEnumerator TransitionRoutine(GameObject targetPanel, CinemachineCamera targetVcam, GameObject firstSelect)
        {
            // Amagar tots els panells
            HideAllPanels();

            // Iniciar el moviment de la càmera
            cameraManager.ActivateCamera(targetVcam);

            // Esperar mentre la càmera es mou
            yield return new WaitForSeconds(1.5f);

            // Mostrar panell i donar focus
            ExecuteSwitch(targetPanel, targetVcam, firstSelect);
        }

        private void ExecuteSwitch(GameObject targetPanel, CinemachineCamera targetVcam, GameObject firstSelect)
        {
            // Activar CameraManager
            cameraManager.ActivateCamera(targetVcam);

            // Control de visibilitat
            starterPanel.SetActive(starterPanel == targetPanel);
            initialMenuPanel.SetActive(initialMenuPanel == targetPanel);
            itemsPanel.SetActive(itemsPanel == targetPanel);
            settingsPanel.SetActive(settingsPanel == targetPanel);
            buildPanel.SetActive(buildPanel == targetPanel);

            // Focus del teclat
            if (firstSelect != null)
            {
                EventSystem.current.SetSelectedGameObject(firstSelect);
            }
        }

        private void HideAllPanels()
        {
            starterPanel.SetActive(false);
            initialMenuPanel.SetActive(false);
            itemsPanel.SetActive(false);
            settingsPanel.SetActive(false);
            buildPanel.SetActive(false);
        }
    }
}