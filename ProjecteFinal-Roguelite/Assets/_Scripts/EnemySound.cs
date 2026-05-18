using Roguelite.Systems;
using UnityEngine;

namespace Roguelite
{
    public class EnemySound : MonoBehaviour
    {
        public void PlayDeath() => AudioManager.Instance.PlaySoundAtPoint(SoundType.enemyDeath, transform.position);
    }
}
