using UnityEngine;
using TMPro;

namespace Roguelite.Player
{
    public class PlayerLevelManager : MonoBehaviour
    {
        public static PlayerLevelManager Instance;

        [SerializeField] private int _currentPlayerLevel = 1; // Nivell actual
        [SerializeField] private TextMeshProUGUI _levelText;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            UpdateGUI();
        }

        public int GetPlayerLevel() => _currentPlayerLevel;

        public void SetPlayerLevel(int newLevel)
        {
            _currentPlayerLevel = newLevel;
            UpdateGUI();
        }

        private void UpdateGUI()
        {
            if (_levelText != null)
            {
                _levelText.text = "Nivell: " + _currentPlayerLevel;
            }
        }
    }
}
