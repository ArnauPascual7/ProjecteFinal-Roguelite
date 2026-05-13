using Roguelite.Behaviours;
using UnityEngine;

namespace Roguelite.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _skeletons;
        [SerializeField] private GameObject[] _meleeWeapons;
        [SerializeField] private GameObject[] _rangedWeapons;

        [SerializeField] private GameObject _enemyTarget;

        private void Start()
        {
            SpawnSkeleton(transform.position);
        }

        private void SpawnSkeleton(Vector3 position)
        {
            int index = Random.Range(0, _skeletons.Length);
            bool isMelee = /*Random.value > 0.5f*/true;

            GameObject skeleton = Instantiate(_skeletons[index], position, Quaternion.identity);
            skeleton.SetActive(false);

            EnemyController controller = skeleton.GetComponent<EnemyController>();
            EnemyData data = new(EnemyDifficulty.Medium);
            data.target = _enemyTarget;

            if (isMelee)
            {
                int meleeIndex = Random.Range(0, _meleeWeapons.Length);
                GameObject go = Instantiate(_meleeWeapons[meleeIndex], skeleton.transform);
                EnemyMeleeWeapon meleeWeapon = go.GetComponent<EnemyMeleeWeapon>();
                go.transform.localPosition = Vector3.zero + new Vector3(0, -0.2f);

                MeleeAttackBehaviour mab = skeleton.AddComponent<MeleeAttackBehaviour>();
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
