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
        //[SerializeField] private Image _iconImage;
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
            //
            Debug.Log("Description: " + desc);

            if (_titleText != null)
            {
                _titleText.text = name;
            }

            if (_descriptionText != null)
            {
                _descriptionText.text = desc;
            }
            /*
            if (_iconImage != null)
            {
                _iconImage.sprite = icon;
                _iconImage.enabled = (icon != null);
            }
            */
            //

            /*
            _titleText.text = name;
            _descriptionText.text = desc;
            _costText.text = $"COST: {cost}";
            _iconImage.sprite = icon; */

            // Lògica de comprovació de diners del jugador
            // _buyButton.interactable = (currentMoney >= cost);
        }

        public void ClearDetails()
        {
            _titleText.text = "Selecciona una millora";
            _descriptionText.text = "";
            _costText.text = "";
            //_iconImage.enabled = false;
        }
    }
}
