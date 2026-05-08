using Roguelite.Player;
using UnityEngine;

namespace Roguelite.Systems
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public PlayerController PlayerController { get; private set; }

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
            }
        }

        public void PlayerControllerInit(PlayerController pc)
        {
            if (PlayerController != null)
            {
                PlayerController = pc;
            }
        }
    }
}
