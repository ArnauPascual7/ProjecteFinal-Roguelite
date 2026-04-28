using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Roguelite.UI
{
    public class UpgradeDetailUI : MonoBehaviour
    {
        public static UpgradeDetailUI Instance;

        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private TextMeshProUGUI _costText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private Button _buyButton;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            ClearDetails();
        }

        public void DisplayUpgrade(string name, string desc, int cost, Sprite icon)
        {
            _titleText.text = name;
            _descriptionText.text = desc;
            _costText.text = $"COST: {cost}";
            _iconImage.sprite = icon;

            // Lògica de comprovació de diners del jugador
            // _buyButton.interactable = (currentMoney >= cost);
        }

        public void ClearDetails()
        {
            _titleText.text = "Selecciona una millora";
            _descriptionText.text = "";
            _costText.text = "";
            _iconImage.enabled = false;
        }
    }
}
