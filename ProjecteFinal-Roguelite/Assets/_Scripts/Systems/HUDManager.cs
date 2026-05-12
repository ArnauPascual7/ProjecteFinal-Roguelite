using Roguelite.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Roguelite.HealthHeartBar;

namespace Roguelite.Systems
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager Instance { get; private set; }

        [Header("Player HUD")]
        [SerializeField] private TextMeshProUGUI _pHealthText;
        [SerializeField] private TextMeshProUGUI _pMagicPowerText;
        [SerializeField] private TextMeshProUGUI _pEnergyText;
        [SerializeField] private TextMeshProUGUI _pExperienceText;

        public float Health => _health;

        private string PlayerHealthText { get => _pHealthText.text; set => _pHealthText.text = value; }
        private string PlayerMagicPowerText { get => _pMagicPowerText.text; set => _pMagicPowerText.text = value; }
        private string PlayerEnergyText { get => _pEnergyText.text; set => _pEnergyText.text = value; }
        private string PlayerExperienceText { get => _pExperienceText.text; set => _pExperienceText.text = value; }

        private float _health;

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
            PlayerController.OnHealthChange += UpdateHUDHealth;
            PlayerController.OnStaminaChange += UpdateHUDEnergy;
            PlayerController.OnMagicPointsChange += UpdateHUDMagicPower;
        }

        private void OnDisable()
        {
            PlayerController.OnHealthChange -= UpdateHUDHealth;
            PlayerController.OnStaminaChange -= UpdateHUDEnergy;
            PlayerController.OnMagicPointsChange -= UpdateHUDMagicPower;
        }

        private void UpdateHUDHealth(float health)
        {
            _health = health;
            PlayerHealthText = $"{health}";
        }
        private void UpdateHUDMagicPower(float magicPower)
        {
            PlayerMagicPowerText = $"{magicPower}";
        }
        private void UpdateHUDEnergy(float energy)
        {
            PlayerEnergyText = $"{energy}";
        }
        private void UpdateHUDExperience(int experience)
        {
            PlayerExperienceText = $"{experience}";
        }
    }
}
