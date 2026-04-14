using UnityEngine;
using UnityEngine.EventSystems;

namespace Roguelite.UI
{
    public class MenuUIManager : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject starterPanel;
        [SerializeField] private GameObject initialMenuPanel;
        [SerializeField] private GameObject itemsPanel;
        [SerializeField] private GameObject settingsPanel;

        [Header("First Selection")]
        [SerializeField] private GameObject firstButtonInitialMenu;

        private void Start()
        {
            ShowStarter();
        }

        public void ShowStarter()
        {
            SetPanelActive(starterPanel);
        }

        public void ShowInitialMenu()
        {
            SetPanelActive(initialMenuPanel);
            // Donar el focus al primer botó per al New Input System
            EventSystem.current.SetSelectedGameObject(firstButtonInitialMenu);
        }

        public void ShowItems()
        {
            SetPanelActive(itemsPanel);
        }
        public void ShowSettings()
        {
            SetPanelActive(settingsPanel);
        }

        private void SetPanelActive(GameObject activePanel)
        {
            starterPanel.SetActive(starterPanel == activePanel);
            initialMenuPanel.SetActive(initialMenuPanel == activePanel);
            itemsPanel.SetActive(itemsPanel == activePanel);
            settingsPanel.SetActive(settingsPanel == activePanel);
        }

        public void QuitGame()
        {
            Debug.Log("Exit...");
            Application.Quit();
        }
    }
}

