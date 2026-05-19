using UnityEngine;
using UnityEngine.SceneManagement;

namespace Roguelite.Navigation
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

            //
            if (Roguelite.Systems.SaveManager.Instance != null)
            {
                Roguelite.Systems.SaveManager.Instance.SaveAll();
            }
                
            //

            SceneManager.LoadScene(sceneName);
        }

        public void ReloadCurrentScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
                //UnityEditor.EditorApplication.isPlaying = false;
                Debug.Log("Exit Game!");
            #else
                Application.Quit();
            #endif
        }
    }
}

