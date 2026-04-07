using UnityEngine;

namespace Roguelite.Behaviours
{
    public class ProjectileFiringBehaviour : MonoBehaviour
    {
        public float fireRate = 1f;
        private float _nextFireTime = 0f;
        public void FireProjectile(GameObject projectilePrefab, Transform spawner, bool fire)
        {
            if (fire && Time.time >= _nextFireTime)
            {
                _nextFireTime = Time.time + fireRate;

                GameObject projectile = Instantiate(projectilePrefab);
                projectile.transform.position = spawner.position;
                projectile.transform.rotation = transform.rotation;
                Destroy(projectile, 2f);    
            }
        }
    }
}