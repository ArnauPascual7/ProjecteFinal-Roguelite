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

        private void Start()
        {
            ShowStarter();
        }

        // Menú principi
        public void ShowStarter()
        {
            SwitchState(starterPanel, vcamStarterInitial, null);
        }

        // Menú inicial
        public void ShowInitialMenu()
        {
            SwitchState(initialMenuPanel, vcamStarterInitial, firstButtonInitialMenu);
        }

        // Menú items
        public void ShowItems()
        {
            SwitchState(itemsPanel, vcamItems, null);
        }

        // Menú configuració
        public void ShowSettings()
        {
            SwitchState(settingsPanel, vcamSettings, null);
        }

        // Menú build
        public void ShowBuild()
        {
            SwitchState(buildPanel, vcamBuild, null);
        }

        private void SwitchState(GameObject targetPanel, CinemachineCamera targetVcam, GameObject firstSelect)
        {
            // Activar CameraManager
            cameraManager.ActivateCamera(targetVcam);

            // Visibilitat dels panells
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
    }
}

