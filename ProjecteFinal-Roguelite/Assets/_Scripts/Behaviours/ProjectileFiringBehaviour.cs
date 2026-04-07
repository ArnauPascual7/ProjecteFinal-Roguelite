using UnityEngine;

namespace Roguelite.Behaviours
{
    public class ProjectileFiringBehaviour : MonoBehaviour
    {
        public void FireProjectile(GameObject projectilePrefab, Transform spawner, bool fire)
        {
            if (fire)
            {
                GameObject projectile = Instantiate(projectilePrefab);
                projectile.transform.position = spawner.position;
                projectile.transform.rotation = transform.rotation;
            }
        }
    }
}