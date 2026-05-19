using System;
using Roguelite.Behaviours;
using Roguelite.BehaviourTree;
using Roguelite.Weapons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Roguelite.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Enemy Skin & Weapons")]
        [SerializeField] private GameObject[] _skeletons;
        [SerializeField] private GameObject[] _meleeWeapons;
        [SerializeField] private EnemyRangedWeapon[] _rangedWeapons;

        [Header("Enemy Settings")]
        [SerializeField] private GameObject _enemyTarget;
        [SerializeField] private ParentStateSO _behaviourTree;
        [SerializeField] private float baseHealth = 300f;
        [SerializeField] private float healthVariation = 1f;
        [SerializeField] private float baseSpeed = 3f;
        [SerializeField] private float speedVariation = 1f;

        public void SpawnAt(Vector3 position)
        {
            SpawnSkeleton(position);
        }

        private void SpawnSkeleton(Vector3 position)
        {
            int skinIndex = Random.Range(0, _skeletons.Length);
            bool isMelee = Random.value > 0.1f;

            GameObject skeleton = Instantiate(_skeletons[skinIndex], position, Quaternion.identity);
            skeleton.SetActive(false);

            EnemyController controller = skeleton.AddComponent<EnemyController>();
            controller.root = _behaviourTree;

            EnemyHealth health = skeleton.GetComponent<EnemyHealth>();
            health.Health = baseHealth * (Random.value * (healthVariation * 2));

            TargetDetectionBehaviour tdb = skeleton.GetComponent<TargetDetectionBehaviour>();
            tdb.target = _enemyTarget;

            if (isMelee)
            {
                skeleton.AddComponent<ChaseBehaviour>();
                MeleeAttackBehaviour mab = skeleton.AddComponent<MeleeAttackBehaviour>();

                int meleeIndex = Random.Range(0, _meleeWeapons.Length);
                GameObject go = Instantiate(_meleeWeapons[meleeIndex], skeleton.transform);

                EnemyMeleeWeapon meleeWeapon = go.GetComponent<EnemyMeleeWeapon>();
                go.transform.localPosition = Vector3.zero + new Vector3(0, -0.2f);

                mab.SetAttackBox(new Vector2(0.9f, 1.6f), -0.3f);
                mab.damage = meleeWeapon.damage;
                mab.target = _enemyTarget;
            }
            else
            {
                EnemyWeapon weapon = skeleton.AddComponent<EnemyWeapon>();

                skeleton.AddComponent<AimRotationBehaviour>();
                skeleton.AddComponent<ProjectileFiringBehaviour>();

                int rangedIndex = Random.Range(0, _rangedWeapons.Length);
                GameObject rangedWeapon = Instantiate(_rangedWeapons[rangedIndex].gameObject, skeleton.transform);

                weapon.SetWeapons(_rangedWeapons[rangedIndex].scriptableObject);
                weapon.shootPoint = rangedWeapon.transform;
                rangedWeapon.transform.localPosition = Vector3.zero;

                if (Random.value > 0.8)
                {
                    skeleton.AddComponent<ChaseBehaviour>();
                }
            }

            if (controller.TryGetComponent<MoveBehaviour>(out var mb))
            {
                mb.baseSpeed = baseSpeed * (Random.value * (speedVariation * 2));
            }

            controller.InitializeEnemy();
            skeleton.SetActive(true);
        }
    }

    [Serializable]
    public struct EnemyRangedWeapon
    {
        public GameObject gameObject;
        public RangedWeapon[] scriptableObject;
    }
}
