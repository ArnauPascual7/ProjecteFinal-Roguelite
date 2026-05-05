using Roguelite.Player;
using TMPro;
using UnityEngine;

namespace Roguelite.Systems
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Player HUD")]
        [SerializeField] private TextMeshProUGUI _pHealthText;
        [SerializeField] private TextMeshProUGUI _pMagicPowerText;
        [SerializeField] private TextMeshProUGUI _pEnergyText;
        [SerializeField] private TextMeshProUGUI _pExperienceText;

        private string PlayerHealthText { get => _pHealthText.text; set => _pHealthText.text = value; }
        private string PlayerMagicPowerText { get => _pMagicPowerText.text; set => _pMagicPowerText.text = value; }
        private string PlayerEnergyText { get => _pEnergyText.text; set => _pEnergyText.text = value; }
        private string PlayerExperienceText { get => _pExperienceText.text; set => _pExperienceText.text = value; }

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

        private void OnEnable()
        {
            PlayerHealth.OnHealthChange += UpdateHUDHealth;
        }

        private void OnDisable()
        {
            PlayerHealth.OnHealthChange -= UpdateHUDHealth;
        }

        private void UpdateHUDHealth(float health)
        {
            PlayerHealthText = $"{health}";
        }
    }
}
