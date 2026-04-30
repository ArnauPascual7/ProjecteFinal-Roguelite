using Roguelite.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        for (int i = 0; i < _pipButtons.Length; i++)
        {
            // Comprovar si el nivell existeix per a aquesta millora
            if (i >= _data.values.Length)
            {
                // Si el nivell no existeix la taula, amagar
                _pipButtons[i].gameObject.SetActive(false);
                continue;
            }

            // Si existeix, activar
            _pipButtons[i].gameObject.SetActive(true);
            ColorBlock cb = _pipButtons[i].colors;

            // Colors i estats
            if (i < _currentLevel) // Nivell ja comprat
            {
                cb.normalColor = Color.cyan;
                cb.disabledColor = Color.cyan;
                _pipButtons[i].interactable = false; // Ja no es pot tornar a comprar
            }
            else if (i == _currentLevel) // Nivell disponible per comprar
            {
                cb.normalColor = Color.white;
                _pipButtons[i].interactable = true;
            }
            else // Nivell bloquejat (s'ha de comprar primer l'anterior)
            {
                cb.normalColor = new Color(0.15f, 0.15f, 0.15f); // Gris fosc
                cb.disabledColor = new Color(0.15f, 0.15f, 0.15f);
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
