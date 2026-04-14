using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hellcore.Navigation
{
    public class SceneLoader : MonoBehaviour
    {
        // Funció que es conecta als botons
        public void LoadScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.Log("ERROR: Scene Name is null or empty.");
                return;
            }
            SceneManager.LoadScene(sceneName);
        }

        public void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
            #else
                            Application.Quit();
            #endif
        }
    }
}

