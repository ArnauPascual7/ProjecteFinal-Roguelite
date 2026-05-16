using UnityEngine;
using UnityEngine.SceneManagement;

namespace Roguelite.Management
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager Instance;

        [Header("UI References")]
        [SerializeField] private GameObject _pausePanel;

        private bool _isPaused = false;
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
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _isPaused = false;
            Time.timeScale = 1f;

            if (_pausePanel != null)
                _pausePanel.SetActive(false);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;

            Time.timeScale = _isPaused ? 0f : 1f;

            if (_pausePanel != null)
                _pausePanel.SetActive(_isPaused);

            Cursor.visible = _isPaused;
            Cursor.lockState = _isPaused ? CursorLockMode.None : CursorLockMode.Confined;
        }
    }
}