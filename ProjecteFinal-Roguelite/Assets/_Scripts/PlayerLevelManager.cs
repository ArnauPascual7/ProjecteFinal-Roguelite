using UnityEngine;
using TMPro;
using Roguelite.Systems;

namespace Roguelite.Player
{
    public class PlayerLevelManager : MonoBehaviour, ISaveable
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

        public void PopulateSaveData(ref ObjectSaveData data)
        {
            data.playerLevel = _currentPlayerLevel;
        }
        public void LoadFromSaveData(ObjectSaveData data)
        {
            _currentPlayerLevel = data.playerLevel;
            UpdateGUI();
        }
    }
}
