using System;
using Roguelite.Player;
using TMPro;
using UnityEngine;

namespace Roguelite.Systems
{
    public class HUDManager : MonoBehaviour
    {
        public static HUDManager Instance { get; private set; }

        [Header("Player HUD")]
        [SerializeField] private TextMeshProUGUI _pMagicPowerText;
        [SerializeField] private TextMeshProUGUI _pExperienceText;

        [SerializeField] private EnergyBar energyBar;

        public float Health => _health;
        public float MaxHealth => _maxHealth;

        private string PlayerMagicPowerText { get => _pMagicPowerText.text; set => _pMagicPowerText.text = value; }
        private string PlayerExperienceText { get => _pExperienceText.text; set => _pExperienceText.text = value; }

        private float _health;
        private float _maxHealth;
        private PlayerController _playerController;
        private bool _energyBarReady = false;

        public event Action<float> OnHealthChanged;
        public event Action<float> OnMaxHealthChanged;

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
            PlayerController.OnStaminaStart += StartEnergyBar;
            PlayerController.OnHealthChange += UpdateHUDHealth;
            PlayerController.OnMaxHealthChange += UpdateHUDMaxHealth;
            PlayerController.OnStaminaChange += UpdateHUDEnergy;
            PlayerController.OnMagicPointsChange += UpdateHUDMagicPower;
        }

        private void OnDisable()
        {
            PlayerController.OnStaminaStart -= StartEnergyBar;
            PlayerController.OnHealthChange -= UpdateHUDHealth;
            PlayerController.OnMaxHealthChange -= UpdateHUDMaxHealth;
            PlayerController.OnStaminaChange -= UpdateHUDEnergy;
            PlayerController.OnMagicPointsChange -= UpdateHUDMagicPower;
        }
        public void InitPlayer(PlayerController playerController)
        {
            _playerController = playerController;
            energyBar.StartEnergyBar(_playerController.Stamina._maxStamina,
                                     _playerController.Stamina.currentStamina);
            _energyBarReady = true;
        }
        private void StartEnergyBar(float energyMax, float actualEnergy)
        {
            energyBar.StartEnergyBar(energyMax, actualEnergy);
        }
        private void UpdateHUDHealth(float health)
        {
            _health = health;
            OnHealthChanged?.Invoke(health);
        }
        private void UpdateHUDMaxHealth(float maxHealth)
        {
            _maxHealth = maxHealth;
            OnMaxHealthChanged?.Invoke(maxHealth);
        }
        private void UpdateHUDMagicPower(float magicPower)
        {
            PlayerMagicPowerText = $"{magicPower}";
        }
        private void UpdateHUDEnergy(float energy)
        {
            if (!_energyBarReady) return;
            energyBar.UpdateEnergyBar(energy);
        }
        private void UpdateHUDExperience(int experience)
        {
            PlayerExperienceText = $"{experience}";
        }
    }
}