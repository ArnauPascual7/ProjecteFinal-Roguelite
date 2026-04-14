using TMPro;
using UnityEngine;

namespace Hellcore.UI.Effects
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UIAlphaPulsator : MonoBehaviour
    {
        [SerializeField] private float speed = 2.0f;
        private TextMeshProUGUI _textComponent;

        private void Awake() => _textComponent = GetComponent<TextMeshProUGUI>();

        private void Update()
        {
            float alpha = (Mathf.Sin(Time.time * speed) + 1.0f) / 2.0f;
            _textComponent.alpha = alpha;
        }
    }
}
