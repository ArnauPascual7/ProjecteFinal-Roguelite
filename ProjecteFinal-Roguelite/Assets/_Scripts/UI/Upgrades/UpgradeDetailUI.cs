using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Roguelite.Economy;
using Roguelite.Player;

namespace Roguelite.UI
{
    public class UpgradeDetailUI : MonoBehaviour
    {
        public static UpgradeDetailUI Instance;

        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _costText;
        //[SerializeField] private Image _iconImage;
        [SerializeField] private Button _buyButton;

        private UpgradeRowUI _currentSelectedRow;
        private int _inspectedLevelIndex;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ClearDetails();
        }

        public void DisplayUpgrade(string upgradeName, string description, int cost, Sprite icon, UpgradeRowUI sender, int levelIndex)
        {
            // Guardar la referència de qui envia la info
            _currentSelectedRow = sender;
            _inspectedLevelIndex = levelIndex;

            // Actualitzar textos
            _titleText.text = upgradeName;
            _descriptionText.text = description;
            //_iconImage.sprite = icon;
            //_iconImage.enabled = (icon != null);

            // Gestionar estat del botó de compra i text del cost
            UpdateBuyButtonVisuals(cost);
        }

        private void UpdateBuyButtonVisuals(int cost)
        {
            if (_currentSelectedRow == null)
            {
                return;
            }

            // Obtenir nivell del jugador
            int playerLevel = PlayerLevelManager.Instance.GetPlayerLevel();

            int requiredPlayerLevel = _currentSelectedRow.GetRequiredPlayerLevel(_inspectedLevelIndex);

            // Prioritats de bloqueig
            if (_currentSelectedRow.IsMaxLevel()) // Si la millora ja ha arribat al seu límit
            {
                _costText.text = "MAXIMITZAT";
                _costText.color = Color.white;
                _buyButton.interactable = false;
                // 
            }
            else if (playerLevel < requiredPlayerLevel) // Si el jugador no té prou nivell
            {
                _costText.text = $"Requereix Nivell {requiredPlayerLevel}";
                _costText.color = Color.red;
                _buyButton.interactable = false;
            }
            else // Si té nivell comprovar diners
            {
                _costText.text = $"COST: {cost} HC";

                // Només interactuable si té prou diners al CurrencyManager
                bool canAfford = CurrencyManager.Instance.CanAfford(cost);
                _buyButton.interactable = canAfford;
                _costText.color = canAfford ? Color.white : Color.red;
            }
        }

        public void OnBuyClicked()
        {
            if (_currentSelectedRow == null)
            {
                return; 
            }

            // Obtenir cost del nivell que el jugador vol comprar
            int cost = _currentSelectedRow.GetCurrentLevelCost();

            // Comprovar economia
            if (cost != -1 && CurrencyManager.Instance.CanAfford(cost))
            {
                // Restar diners
                CurrencyManager.Instance.Spend(cost);

                // Confirmar compra
                _currentSelectedRow.ConfirmPurchase();

                //int currentLevel = _currentSelectedRow.GetCurrentLevelIndex();

                // Actualitzar panell de detalls amb el següent preu
                //_currentSelectedRow.OnPipClicked(currentLevel < 11 ? currentLevel : 10);

                Debug.Log($"<color=cyan>Millora comprada!</color>");
            }
        }

        public void ClearDetails()
        {
            _titleText.text = "Selecciona una millora";
            _descriptionText.text = "";
            _costText.text = "";
            //_iconImage.enabled = false;
            _buyButton.interactable = false;
            _currentSelectedRow = null;
        }
    }
}
