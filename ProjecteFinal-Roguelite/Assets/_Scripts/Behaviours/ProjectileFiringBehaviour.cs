using System.Collections;
using System.Collections.Generic;
using Roguelite.Weapons;
using UnityEngine;

namespace Roguelite.Behaviours
{
    public class ProjectileFiringBehaviour : MonoBehaviour
    {
        [SerializeField] private int _initialPoolSizePerType = 10;

        private readonly Dictionary<GameObject, Queue<GameObject>> _pools = new();
        private Coroutine _burstCoroutine = null;

        public void FireProjectile(ProjectileWeapon weapon, GameObject projectilePrefab, Transform shootPoint)
        {
            EnsurePool(projectilePrefab);

            if (weapon.projectilesPerShot <= 1)
            {
                SpawnProjectile(weapon, projectilePrefab, shootPoint);
            }
            else
            {
                _burstCoroutine ??= StartCoroutine(FireBurstCoroutine(weapon, projectilePrefab, shootPoint));
            }
        }

        private IEnumerator FireBurstCoroutine(ProjectileWeapon weapon, GameObject projectilePrefab, Transform shootPoint)
        {
            for (int i = 0; i < weapon.projectilesPerShot; i++)
            {
                SpawnProjectile(weapon, projectilePrefab, shootPoint);
                yield return new WaitForSeconds(weapon.timeBetweenProjectiles);
            }
            _burstCoroutine = null;
        }

        private void SpawnProjectile(ProjectileWeapon weapon, GameObject projectilePrefab, Transform shootPoint)
        {
            Queue<GameObject> pool = _pools[projectilePrefab];

            GameObject projectile = pool.Count > 0
                ? pool.Dequeue()
                : Instantiate(projectilePrefab);

            projectile.transform.SetPositionAndRotation(shootPoint.position, shootPoint.rotation);
            projectile.SetActive(true);

            if (projectile.TryGetComponent<ProjectileBehaviour>(out var pb))
            {
                pb.Initialize(this, projectilePrefab, shootPoint, weapon.projectileSpeed,
                              weapon.damage, weapon.force, weapon.range, weapon.projectileLayerName);
            }
        }

        public void ReturnToPool(GameObject prefabKey, GameObject projectile)
        {
            projectile.SetActive(false);

            if (_pools.TryGetValue(prefabKey, out var pool))
                pool.Enqueue(projectile);
        }

        private void EnsurePool(GameObject prefab)
        {
            if (_pools.ContainsKey(prefab)) return;

            var queue = new Queue<GameObject>();
            for (int i = 0; i < _initialPoolSizePerType; i++)
            {
                var go = Instantiate(prefab);
                go.SetActive(false);
                queue.Enqueue(go);
            }
            _pools[prefab] = queue;
        }
    }
}