using UnityEngine;

namespace Roguelite.Enemy
{
    public class EnemyData
    {
        public GameObject target;

        public float health;
        public float speed;

        public EnemyData(EnemyDifficulty diff)
        {
            switch(diff)
            {
                case EnemyDifficulty.Easy:
                    health = GetRandomValue(100f, 300f);
                    speed = GetRandomValue(1f, 2f);
                    break;
                case EnemyDifficulty.Medium:
                    health = GetRandomValue(200f, 400f);
                    speed = GetRandomValue(2f, 3f);
                    break;
                case EnemyDifficulty.Hard:
                    health = GetRandomValue(300f, 500f);
                    speed = GetRandomValue(3f, 4f);
                    break;
            }
        }

        private float GetRandomValue(float min, float max)
        {
            return Random.Range(min, max);
        }
    }

    public enum EnemyDifficulty
    {
        Easy,
        Medium,
        Hard
    }
}
