using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using Roguelite.Systems;

namespace Roguelite.Economy
{
    public class CurrencyManager : MonoBehaviour, ISaveable
    {
        public static CurrencyManager Instance;

        [SerializeField] private int _coins = 1000; // Diners incials per fer proves
        [SerializeField] private TextMeshProUGUI _currencyText;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            UpdateUI();
        }

        public bool CanAfford(int amount)
        {
            return _coins >= amount; 
        }


        public void Spend(int amount)
        {
            _coins -= amount;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (_currencyText != null)
            {
                _currencyText.text = _coins.ToString() + " Monedes";
            }
        }

        public void PopulateSaveData(ref ObjectSaveData data)
        {
            data.coins = _coins;
        }
        public void LoadFromSaveData(ObjectSaveData data)
        {
            _coins = data.coins;
            UpdateUI();
        }
    }
}
