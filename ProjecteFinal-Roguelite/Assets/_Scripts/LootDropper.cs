using UnityEngine;

namespace Roguelite
{
    public class LootDropper : MonoBehaviour
    {
        [Header("Potion Prefabs")]
        [SerializeField] private GameObject smallPotionPrefab;
        [SerializeField] private GameObject mediumPotionPrefab;
        [SerializeField] private GameObject bigPotionPrefab;
        [SerializeField] private GameObject maxPotionPrefab;

        [Header("Drop Chances")]
        [Range(0f, 1f)]
        [SerializeField] private float dropChance = 0.5f;

        private const float MaxPotionThreshold = 0.01f;
        private const float BigPotionThreshold = 0.15f;
        private const float MediumPotionThreshold = 0.40f;

        public void TryDropLoot()
        {
            if (Random.value > dropChance) return;

            float rand = Random.value;

            GameObject prefabToDrop;

            if (rand < MaxPotionThreshold)
                prefabToDrop = maxPotionPrefab;
            else if (rand < BigPotionThreshold)
                prefabToDrop = bigPotionPrefab;
            else if (rand < MediumPotionThreshold)
                prefabToDrop = mediumPotionPrefab;
            else
                prefabToDrop = smallPotionPrefab;

            if (prefabToDrop != null)
                Instantiate(prefabToDrop, transform.position, Quaternion.identity);
        }
    }
}
