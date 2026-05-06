using Roguelite.Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Roguelite.UI
{
    public class UpgradeRowUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private Image _iconImage;
        [SerializeField] private GameObject _levelIndicator;
        [SerializeField] private Button[] _pipButtons;

        [Header("Data Configuration")]
        [SerializeField] private UpgradeData _data;
        [SerializeField] private float _fixedWidth = 450f;

        private int _currentLevel = 0; // Nivell actual comprat      

        private void Start()
        {
            if (_data != null)
            {
                Setup(_data, 0); // Iniciar a nivell 0
            }
        }

        public void Setup(UpgradeData upgradeData, int savedLevel)
        {
            _data = upgradeData;
            _currentLevel = savedLevel;

            if (_nameText != null)
            {
                _nameText.text = _data.upgradeName;
            }
                
            if (_iconImage != null)
            {
                _iconImage.sprite = _data.icon;
            }
                
            UpdateVisuals();
        }

        // Actualitzar botons
        public void UpdateVisuals()
        {
            if (_data == null || _pipButtons == null)
            {
                return; 
            }

            int activeCount = 0;
            int playerLevel = PlayerLevelManager.Instance.GetPlayerLevel();

            for (int i = 0; i < _pipButtons.Length; i++)
            {
                // Activar o desactivar segons si el nivell existeix a la taula
                bool existsLevel = i < _data.values.Length;
                _pipButtons[i].gameObject.SetActive(existsLevel);

                if (!existsLevel)
                {
                    continue;
                }
                activeCount++;

                // Llegir nivell requerit
                int levelRequired = GetRequiredPlayerLevel(i);

                // Comprovar
                bool levelPlayerEnough = playerLevel >= levelRequired;

                // Lògica de colors i interactuabilitat
                ColorBlock cb = _pipButtons[i].colors;

                if (i < _currentLevel) // Nivell ja comprat
                {
                    Color comprat = Color.cyan;
                    cb.normalColor = comprat;
                    cb.disabledColor = comprat;
                    _pipButtons[i].interactable = false;
                }
                else if (i == _currentLevel && levelPlayerEnough) // Nivell disponible per comprar
                {
                    cb.normalColor = Color.white;
                    _pipButtons[i].interactable = true;
                }
                else // Nivell bloquejat
                {
                    Color bloquejat = new Color(0.15f, 0.15f, 0.15f);
                    cb.normalColor = bloquejat;
                    cb.disabledColor = bloquejat;
                    _pipButtons[i].interactable = false;
                }

                _pipButtons[i].colors = cb;
            }

            // Justificar botons
            JustifyButtons(activeCount);
        }

        // Calcular espai entre botons
        private void JustifyButtons(int activeCount)
        {
            if (_levelIndicator == null)
            {
                return;
            }               

            HorizontalLayoutGroup layout = _levelIndicator.GetComponent<HorizontalLayoutGroup>();

            if (layout == null)
            {
                return; 
            }

            if (activeCount <= 1)
            {
                layout.spacing = 0;
                return;
            }

            float totalWidth = _fixedWidth - layout.padding.left - layout.padding.right;
            float buttonsWidth = activeCount * 35f;
            float leftoverSpace = totalWidth - buttonsWidth;
            float calculatedSpacing = leftoverSpace / (activeCount - 1);
            layout.spacing = Mathf.Max(0, calculatedSpacing);

            // Forçar actualització visual
            LayoutRebuilder.ForceRebuildLayoutImmediate(_levelIndicator.GetComponent<RectTransform>());
        }

        public void OnPipClicked(int levelIndex)
        {
            if (_data == null)
            {
                return; 
            }

            // Obtenir cost i descripció del nivell clicat
            int cost = (levelIndex < _data.costs.Length) ? _data.costs[levelIndex] : 0;
            string descriptionToShow = "Sense descripció.";

            if (_data.levelDescriptions != null && _data.levelDescriptions.Length > 0)
            {
                int descIndex = (levelIndex < _data.levelDescriptions.Length) ? levelIndex : 0;
                descriptionToShow = _data.levelDescriptions[descIndex];
            }

            // Enviar al panell d'info
            if (UpgradeDetailUI.Instance != null)
            {
                UpgradeDetailUI.Instance.DisplayUpgrade(_data.upgradeName, descriptionToShow, cost, _data.icon, this, levelIndex);
            }
        }

        public int GetRequiredPlayerLevel(int index)
        {
            if (_data != null && _data.playerLevelRequired != null && index < _data.playerLevelRequired.Length)
            {
                return _data.playerLevelRequired[index];
            }
            return 1; // Nivell 1 per defecte
        }

        // Cost del nivell actual que s'ha de comprar
        public int GetCurrentLevelCost()
        {
            if (_currentLevel < _data.costs.Length)
            {
                return _data.costs[_currentLevel];
            }            
            return -1; // Si ja està al màxim
        }

        // Comprovar si ja ha arribat al nivell màxim
        public bool IsMaxLevel()
        {
            return _currentLevel >= _data.values.Length;
        }

        public int GetCurrentLevelIndex()
        {
            return _currentLevel; 
        }

        // Executar la compra
        public void ConfirmPurchase()
        {
            _currentLevel++;
            UpdateVisuals();

            // Actualitzar el panell de detalls amb el nou preu del següent nivell
            OnPipClicked(_currentLevel < _data.values.Length ? _currentLevel : _currentLevel - 1);
        }
    }
}