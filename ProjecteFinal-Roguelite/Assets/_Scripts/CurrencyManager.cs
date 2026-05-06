using UnityEngine;
using TMPro;
using UnityEngine.Rendering;

namespace Roguelite.Economy
{
    public class CurrencyManager : MonoBehaviour
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
    }
}
