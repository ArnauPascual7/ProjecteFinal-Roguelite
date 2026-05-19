using Roguelite.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Roguelite.Management
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager Instance;

        [Header("UI References")]
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private GameObject _hudPanel;
        [SerializeField] private GameObject _deathPanel;

        private bool _isPaused = false;
        private bool _isDead = false;
        public bool IsPaused => _isPaused;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            PlayerHealth.OnPlayerDeath += HandleDeath;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            PlayerHealth.OnPlayerDeath -= HandleDeath;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _isPaused = false;
            _isDead = false;
            Time.timeScale = 1f;

            if (_pausePanel != null)
                _pausePanel.SetActive(false);

            if (_deathPanel != null) 
                _deathPanel.SetActive(false);

            if (scene.name == "GameMenu") // Si estem a menu, apagar el HUD i mostrar el cursor
            {
                if (_hudPanel != null) _hudPanel.SetActive(false);

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else // Si estem a la escena de joc
            {
                if (_hudPanel != null) _hudPanel.SetActive(true);

                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Confined;
            }
        }

        public void TogglePause()
        {
            if (_isDead) return;

            _isPaused = !_isPaused;

            Time.timeScale = _isPaused ? 0f : 1f;

            if (_pausePanel != null)
                _pausePanel.SetActive(_isPaused);

            Cursor.visible = _isPaused;
            Cursor.lockState = _isPaused ? CursorLockMode.None : CursorLockMode.Confined;
        }

        private void HandleDeath()
        {
            if (_isDead) return;

            _isDead = true;
            StartCoroutine(DeathSequence());
            /*
            Time.timeScale = 0f; // Aturar el joc

            if (_deathPanel != null) _deathPanel.SetActive(true);
            if (_hudPanel != null) _hudPanel.SetActive(false);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("Pantalla de Mort activada");
            */
        }

        private IEnumerator DeathSequence()
        {
            // Esperar un temps perquè l'animació de mort s'executi
            yield return new WaitForSeconds(1.5f);

            // Aturar el joc
            Time.timeScale = 0f;

            // Mostrar la UI
            if (_deathPanel != null) _deathPanel.SetActive(true);
            if (_hudPanel != null) _hudPanel.SetActive(false);

            // Alliberar el cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            _isDead = false;
            // Carregar la escena actual de nou
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void QuitToMenu()
        {
            // Restablir el temps
            Time.timeScale = 1f;
            _isPaused = false;

            // Amagar el panell
            if (_pausePanel != null) _pausePanel.SetActive(false);

            // Tornar al cursor normal
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            // Carregar l'escena del menú
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameMenu");
        }
    }
}