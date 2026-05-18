using Roguelite.Player;
using UnityEngine;

namespace Roguelite
{
    public class HealingItem : MonoBehaviour
    {
        public HealingItemObject healingItem;

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth == null || !playerHealth.IsAlive) return;

            playerHealth.Health += healingItem.healAmount;
            Destroy(gameObject);
        }
    }
}
