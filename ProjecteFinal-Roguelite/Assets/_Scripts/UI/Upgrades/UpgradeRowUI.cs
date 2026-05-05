using Roguelite.UI;
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

        private int _currentLevel = 0; // Nivell actual comprat
        [SerializeField] private float _fixedWidth = 450f;

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

            if (_nameText != null) _nameText.text = _data.upgradeName;
            if (_iconImage != null) _iconImage.sprite = _data.icon;

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

            for (int i = 0; i < _pipButtons.Length; i++)
            {
                // Activar o desactivar segons si el nivell existeix a la taula
                bool existeixNivell = i < _data.values.Length;
                _pipButtons[i].gameObject.SetActive(existeixNivell);

                if (!existeixNivell) continue;

                activeCount++;

                // Lògica de colors i interactuabilitat
                ColorBlock cb = _pipButtons[i].colors;

                if (i < _currentLevel) // Nivell ja comprat
                {
                    Color comprat = Color.cyan;
                    cb.normalColor = comprat;
                    cb.disabledColor = comprat;
                    _pipButtons[i].interactable = false;
                }
                else if (i == _currentLevel) // Nivell disponible per comprar
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
            if (_levelIndicator == null) return;

            HorizontalLayoutGroup layout = _levelIndicator.GetComponent<HorizontalLayoutGroup>();

            if (layout == null) return;

            if (activeCount <= 1)
            {
                layout.spacing = 0;
                return;
            }

            // Fem servir la mida fixada (450) en lloc de preguntar-li al RectTransform,
            // que és el que ens estava donant el valor 0.
            float totalWidth = _fixedWidth - layout.padding.left - layout.padding.right;

            // Ample que ocupen els botons (35px cada un)
            float buttonsWidth = activeCount * 35f;

            // Espai sobrant
            float leftoverSpace = totalWidth - buttonsWidth;

            // Càlcul final de l'espaiat
            float calculatedSpacing = leftoverSpace / (activeCount - 1);

            // Si el càlcul dóna negatiu (perquè no hi caben), posem 0
            layout.spacing = Mathf.Max(0, calculatedSpacing);

            // Forçar actualització visual
            LayoutRebuilder.ForceRebuildLayoutImmediate(_levelIndicator.GetComponent<RectTransform>());
        }
        /*
        private void JustifyButtons(int activeCount)
        {
            if (_levelIndicator == null)
            {
                return;
            }               

            HorizontalLayoutGroup layout = _levelIndicator.GetComponent<HorizontalLayoutGroup>();
            RectTransform containerRect = _levelIndicator.GetComponent<RectTransform>();

            if (layout == null || containerRect == null) 
            {
                return;
            }

            if (activeCount <= 1)
            {
                layout.spacing = 0;
                return;
            }

            // Calcular espai útil (Ample total - paddings)
            float totalWidth = containerRect.rect.width - layout.padding.left - layout.padding.right;

            // Ample que ocupen els botons
            float buttonsWidth = activeCount * 35f;

            // Espai que sobra i repartir entre els buits (activeCount - 1)
            float leftoverSpace = totalWidth - buttonsWidth;
            float calculatedSpacing = leftoverSpace / (activeCount - 1);

            // Aplicar espaiat
            layout.spacing = calculatedSpacing;
        }*/

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
                UpgradeDetailUI.Instance.DisplayUpgrade(_data.upgradeName, descriptionToShow, cost, _data.icon);
            }
        }
    }
}

/*
public class UpgradeRowUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Button[] _pipButtons; // Botons dels 11 nivells

    [SerializeField] private UpgradeData _data;

    private int _currentLevel = 0;

    private void Start()
    {
        if (_data != null)
        {
            SetUp(_data, 0); // Comencem a nivell 0
        }
    }

    public void SetUp(UpgradeData data, int savedLevel)
    {
        _data = data;
        _currentLevel = savedLevel;
        _nameText.text = _data.upgradeName;
        _iconImage.sprite = _data.icon;

        UpdateVisuals();
    }

    public void UpdateVisuals()
    {
        int activeCount = 0;

        for (int i = 0; i < _pipButtons.Length; i++)
        {
            // Activar o desactivar segons si el nivell existeix a la taula
            bool existeixNivell = i < _data.values.Length;
            _pipButtons[i].gameObject.SetActive(existeixNivell);

            if (!existeixNivell) continue;

            activeCount++;

            // Lògica de colors i interactuabilitat
            ColorBlock cb = _pipButtons[i].colors;

            if (i < _currentLevel) // Nivell ja comprat
            {
                Color comprat = Color.cyan;
                cb.normalColor = comprat;
                cb.disabledColor = comprat;
                _pipButtons[i].interactable = false;
            }
            else if (i == _currentLevel) // Nivell disponible per comprar
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
    }

    public void OnPipClicked(int levelIndex)
    {
        // Calcular cost del nivell que s'ha clicat
        int cost = _data.costs[levelIndex];

        // Descripció dinàmica
        string descriptionToShow = "Sense descripció";

        if (_data.levelDescriptions != null && _data.levelDescriptions.Length > 0)
        {
            // Si hi ha descripció del nivell, mostrar. Si no, mostrar genèrica
            int descIndex = (levelIndex < _data.levelDescriptions.Length) ? levelIndex : 0;
            descriptionToShow = _data.levelDescriptions[descIndex];
        }

        // Enviar info al panell de la dreta
        if (UpgradeDetailUI.Instance != null)
        {
            UpgradeDetailUI.Instance.DisplayUpgrade(_data.upgradeName, descriptionToShow, cost, _data.icon);
        }

        // Lògica de restar experiència
        //Debug.Log($"Trying to buy {_data.upgradeName} Level {levelIndex + 1}");
        //_currentLevel++;
        //UpdateVisuals();
    }
}
*/