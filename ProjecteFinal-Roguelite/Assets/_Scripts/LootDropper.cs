using UnityEngine;

namespace Roguelite
{
    public class LootDropper : MonoBehaviour
    {
        [Header("Drop Chances")]
        [Range(0f, 1f)]
        [SerializeField] private float dropChance = 0.5f;

        private const float MaxPotionThreshold = 0.01f;
        private const float BigPotionThreshold = 0.15f;
        private const float MediumPotionThreshold = 0.40f;

        private GameObject[] _potionPrefabs; // 0=Small, 1=Medium, 2=Big, 3=Max

        private void Awake()
        {
            _potionPrefabs = new GameObject[]
            {
                Resources.Load<GameObject>("Potions/Small_Potion"),
                Resources.Load<GameObject>("Potions/Medium_Potion"),
                Resources.Load<GameObject>("Potions/Big_Potion"),
                Resources.Load<GameObject>("Potions/Max_Potion"),
            };
        }

        public void TryDropLoot()
        {
            if (Random.value > dropChance) return;

            float rand = Random.value;

            GameObject prefabToDrop;

            if (rand < MaxPotionThreshold)
                prefabToDrop = _potionPrefabs[3]; // Max
            else if (rand < BigPotionThreshold)
                prefabToDrop = _potionPrefabs[2]; // Big
            else if (rand < MediumPotionThreshold)
                prefabToDrop = _potionPrefabs[1]; // Medium
            else
                prefabToDrop = _potionPrefabs[0]; // Small

            if (prefabToDrop != null)
                Instantiate(prefabToDrop, transform.position, Quaternion.identity);
            else
                Debug.LogWarning("[LootDropper] Prefab no encontrado en Resources/Potions/");
        }
    }
}
