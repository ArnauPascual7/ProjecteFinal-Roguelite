using System.Collections;
using System.Collections.Generic;
using Roguelite.Weapons;
using UnityEngine;

namespace Roguelite.Behaviours
{
    public class ProjectileFiringBehaviour : MonoBehaviour
    {
        public Stack<GameObject> ProjectileStack = new Stack<GameObject>();

        private Coroutine _burstCoroutine = null;

        public void FireProjectile(ProjectileWeapon weapon, GameObject projectilePrefab, Transform shootPoint)
        {
            if (weapon.projectilesPerShot <= 1)
            {
                if (ProjectileStack.Count == 0)
                {
                    SpawnProjectile(weapon, projectilePrefab, shootPoint);
                }
                else
                {
                    ProjectileStackPop(weapon, shootPoint);
                }
            }
            else
            {
                // No interrompem un burst en curs
                _burstCoroutine ??= StartCoroutine(FireBurstCoroutine(weapon, projectilePrefab, shootPoint));
            }
        }

        private IEnumerator FireBurstCoroutine(ProjectileWeapon weapon, GameObject projectilePrefab, Transform shootPoint)
        {
            for (int i = 0; i < weapon.projectilesPerShot; i++)
            {
                if (ProjectileStack.Count == 0)
                {
                    SpawnProjectile(weapon, projectilePrefab, shootPoint);
                }
                else
                {
                    ProjectileStackPop(weapon, shootPoint);
                }

                yield return new WaitForSeconds(weapon.timeBetweenProjectiles);
            }

            _burstCoroutine = null;
        }

        private void SpawnProjectile(ProjectileWeapon weapon, GameObject projectilePrefab, Transform shootPoint)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);

            if (projectile.TryGetComponent<ProjectileBehaviour>(out var pb))
            {
                pb.Initialize(this, shootPoint, weapon.projectileSpeed, weapon.damage, weapon.force, weapon.range, weapon.projectileLayerName);
            }
        }

        public void ProjectileStackPush(GameObject go)
        {
            ProjectileStack.Push(go);
            go.SetActive(false);
        }

        private void ProjectileStackPop(ProjectileWeapon weapon, Transform shootPoint)
        {
            GameObject go = ProjectileStack.Pop();

            if (go.TryGetComponent<ProjectileBehaviour>(out var pb))
            {
                pb.Initialize(this, shootPoint, weapon.projectileSpeed, weapon.damage, weapon.force, weapon.range, weapon.projectileLayerName);
            }
            go.SetActive(true);
        }
    }
}