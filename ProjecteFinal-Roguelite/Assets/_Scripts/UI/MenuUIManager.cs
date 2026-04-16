using Roguelite.camera;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Roguelite.UI
{
    public class MenuUIManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private MenuCameraController cameraController;

        [Header("Panels")]
        [SerializeField] private GameObject starterPanel;
        [SerializeField] private GameObject initialMenuPanel;
        [SerializeField] private GameObject itemsPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject buildPanel;

        [Header("Waypoints")]
        [SerializeField] private Transform wpStarterInitial;
        [SerializeField] private Transform wpItems;
        [SerializeField] private Transform wpSettings;
        [SerializeField] private Transform wpBuild;

        [Header("First Selection")]
        [SerializeField] private GameObject firstButtonInitialMenu;

        private void Start()
        {
            ShowStarter();
        }

        // Menú principi
        public void ShowStarter()
        {
            /*
            SetPanelActive(starterPanel);
            */
            SwitchState(starterPanel, wpStarterInitial, null);
        }

        // Menú inicial
        public void ShowInitialMenu()
        {
            /*
            SetPanelActive(initialMenuPanel);
            // Donar el focus al primer botó per al New Input System
            EventSystem.current.SetSelectedGameObject(firstButtonInitialMenu);
            */
            SwitchState(initialMenuPanel, wpStarterInitial, firstButtonInitialMenu);
        }

        // Menú items
        public void ShowItems()
        {
            /*
            SetPanelActive(itemsPanel);
            */
            SwitchState(itemsPanel, wpItems, null);
        }

        // Menú configuració
        public void ShowSettings()
        {
            /*
            SetPanelActive(settingsPanel);
            */
            SwitchState(settingsPanel, wpSettings, null);
        }

        // Menú build
        public void ShowBuild()
        {
            /*
            SetPanelActive(buildPanel);
            */
            SwitchState(buildPanel, wpBuild, null);
        }

        private void SwitchState(GameObject panel, Transform cameraWayPoint, GameObject firstSelect)
        {
            // Moure la càmera
            cameraController.MoveToPosition(cameraWayPoint);

            // Gestió de panells
            starterPanel.SetActive(starterPanel == panel);
            initialMenuPanel.SetActive(initialMenuPanel == panel);
            itemsPanel.SetActive(itemsPanel == panel);
            settingsPanel.SetActive(settingsPanel == panel);
            buildPanel.SetActive(buildPanel == panel);

            // Focus del teclat
            if (firstSelect != null)
            {
                EventSystem.current.SetSelectedGameObject(firstSelect);
            }
        }
        /*
        private void SetPanelActive(GameObject activePanel)
        {
            starterPanel.SetActive(starterPanel == activePanel);
            initialMenuPanel.SetActive(initialMenuPanel == activePanel);
            itemsPanel.SetActive(itemsPanel == activePanel);
            settingsPanel.SetActive(settingsPanel == activePanel);
            buildPanel.SetActive(buildPanel == activePanel);
        }

        public void QuitGame()
        {
            Debug.Log("Exit...");
            Application.Quit();
        }
        */
    }
}

