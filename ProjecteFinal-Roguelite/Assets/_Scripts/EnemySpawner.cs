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

            GameObject skeleton = Instantiate(_skeletons[index], position, Quaternion.identity);
            EnemyController controller = skeleton.GetComponent<EnemyController>();
            EnemyData data = new(EnemyDifficulty.Medium);
            data.target = _enemyTarget;


            controller.InitializeEnemy(data);
        }
    }
}
