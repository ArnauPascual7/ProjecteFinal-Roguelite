using Roguelite.Behaviours;
using Roguelite.BehaviourTree;
using UnityEngine;

namespace Roguelite.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _skeletons;
        [SerializeField] private GameObject[] _meleeWeapons;
        [SerializeField] private GameObject[] _rangedWeapons;

        [SerializeField] private GameObject _enemyTarget;
        [SerializeField] private ParentStateSO _behaviourTree;

        private void Start()
        {
            SpawnSkeleton(transform.position);
        }

        private void SpawnSkeleton(Vector3 position)
        {
            int skinIndex = Random.Range(0, _skeletons.Length);
            bool isMelee = /*Random.value > 0.2f*/true;

            GameObject skeleton = Instantiate(_skeletons[skinIndex], position, Quaternion.identity);
            skeleton.SetActive(false);

            EnemyController controller = skeleton.AddComponent<EnemyController>();
            controller.root = _behaviourTree;

            EnemyData data = new(EnemyDifficulty.Medium)
            {
                target = _enemyTarget
            };

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
                int rangedIndex = Random.Range(0, _rangedWeapons.Length);
                GameObject rangedWeapon = Instantiate(_rangedWeapons[rangedIndex], skeleton.transform);
                rangedWeapon.transform.localPosition = Vector3.zero;
            }

            controller.InitializeEnemy(data);
            skeleton.SetActive(true);
        }
    }
}
