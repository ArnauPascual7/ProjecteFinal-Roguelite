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

        List<HealthHeart> hearts = new List<HealthHeart>();

        private void Start()
        {
            HUDManager.Instance.OnMaxHealthChanged += BuildHearts;
            HUDManager.Instance.OnHealthChanged += UpdateHearts;
        }

        private void OnDestroy()
        {
            if (HUDManager.Instance != null)
            {
                HUDManager.Instance.OnMaxHealthChanged -= BuildHearts;
                HUDManager.Instance.OnHealthChanged -= UpdateHearts;
            }
        }
        private void BuildHearts(float maxHealth)
        {
            ClearHearts();
            int heartsToMake = Mathf.CeilToInt(maxHealth);
            for (int i = 0; i < heartsToMake; i++)
                CreateEmptyHeart();

            UpdateHearts(HUDManager.Instance.Health);
        }

        private void UpdateHearts(float health)
        {
            for (int i = 0; i < hearts.Count; i++)
            {
                float heartValue = health - i;

                if (heartValue >= 1f)
                    hearts[i].SetHeartImage(HeartStatus.Full);
                else if (heartValue >= 0.5f)
                    hearts[i].SetHeartImage(HeartStatus.Half);
                else
                    hearts[i].SetHeartImage(HeartStatus.Empty);
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
