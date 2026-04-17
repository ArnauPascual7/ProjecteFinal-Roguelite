using System.Collections; // Necessari per a les Corrutines
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
            // El Starter es mostra immediatament sense cap transició
            ShowStarter();
        }

        // --- FUNCIONS PÚBLIQUES ---

        public void ShowStarter()
        {
            // Crida directa sense espera
            ExecuteSwitch(starterPanel, vcamStarterInitial, null);
        }

        public void ShowInitialMenu()
        {
            // Si el starter està actiu, vol dir que és la primera transició
            // Per tant, no fem servir retard (segons la teva petició)
            bool skipDelay = starterPanel.activeSelf;

            if (skipDelay)
                ExecuteSwitch(initialMenuPanel, vcamStarterInitial, firstButtonInitialMenu);
            else
                StartTransition(initialMenuPanel, vcamStarterInitial, firstButtonInitialMenu);
        }

        public void ShowItems() => StartTransition(itemsPanel, vcamItems, null);
        public void ShowSettings() => StartTransition(settingsPanel, vcamSettings, null);
        public void ShowBuild() => StartTransition(buildPanel, vcamBuild, null);

        // --- LÒGICA DE TRANSICIÓ ---

        private void StartTransition(GameObject targetPanel, CinemachineCamera targetVcam, GameObject firstSelect)
        {
            // Si hi ha una transició en marxa, l'aturem per evitar errors
            if (_transitionCoroutine != null) StopCoroutine(_transitionCoroutine);

            _transitionCoroutine = StartCoroutine(TransitionRoutine(targetPanel, targetVcam, firstSelect));
        }

        private IEnumerator TransitionRoutine(GameObject targetPanel, CinemachineCamera targetVcam, GameObject firstSelect)
        {
            // 1. Amaguem tots els panells perquè es vegi el Tileset net
            HideAllPanels();

            // 2. Iniciem el moviment de la càmera
            cameraManager.ActivateCamera(targetVcam);

            // 3. ESPEREM 1.5 segons mentre la càmera "vola"
            yield return new WaitForSeconds(1.5f);

            // 4. Mostrem el panell de destí i donem el focus
            ExecuteSwitch(targetPanel, targetVcam, firstSelect);
        }

        private void ExecuteSwitch(GameObject targetPanel, CinemachineCamera targetVcam, GameObject firstSelect)
        {
            // Activar CameraManager (en el cas de l'starter/initial, s'executa aquí directament)
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