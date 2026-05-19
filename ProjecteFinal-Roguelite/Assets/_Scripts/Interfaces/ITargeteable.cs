using UnityEngine;

namespace Roguelite.Interfaces
{
    public interface ITargeteable
    {
        public float Health { get; set; }

        public void TakeDamage(float damage);

        public void Die();
    }
}