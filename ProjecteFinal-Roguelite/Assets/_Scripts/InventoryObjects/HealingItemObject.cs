using UnityEngine;

namespace Roguelite
{
    [CreateAssetMenu(fileName = "HealingItem", menuName = "Items/Healing Item")]
    public class HealingItemObject : ItemObject
    {
        [Range(0.5f, 12f)]
        public float healAmount = 1f;
    }
}
