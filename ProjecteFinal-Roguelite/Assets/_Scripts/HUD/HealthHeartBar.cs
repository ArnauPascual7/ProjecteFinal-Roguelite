using Roguelite.Player;
using Roguelite.Systems;
using System.Collections.Generic;
using UnityEngine;
using static Roguelite.HealthHeart;

namespace Roguelite
{
    public class HealthHeartBar : MonoBehaviour
    {
        public GameObject heartPrefab;
        
        private PlayerHealth _playerHealth;

        List<HealthHeart> hearts = new List<HealthHeart>();

        private void Awake()
        {
            _playerHealth = GetComponent<PlayerHealth>();
        }
        private void Start()
        {
            DrawHearts();
        }

        public void DrawHearts()
        {
            ClearHearts();

            int heartsToMake = (int)(HUDManager.Instance.Health);
            for (int i = 0; i < heartsToMake; i++)
            {
                CreateEmptyHeart();
            }
        }

        public void CreateEmptyHeart()
        {
            GameObject newHeart = Instantiate(heartPrefab);
            newHeart.transform.SetParent(transform);

            HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
            heartComponent.SetHeartImage(HeartStatus.Empty);
            hearts.Add(heartComponent);

        }

        public void ClearHearts()
        {
            foreach(Transform t in transform)
            {
                Destroy(t.gameObject);
            }
            hearts = new List<HealthHeart>();
        }
    }
}
