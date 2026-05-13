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
            Debug.Log("Start()");
            DrawHearts();
        }

        public void DrawHearts()
        {
            Debug.Log("DrawHearts");
            ClearHearts();
            Debug.Log("DrawHearts2");
            int heartsToMake = (int)(3 /*HUDManager.Instance.Health*/);
            Debug.Log(heartsToMake);
            for (int i = 0; i < heartsToMake; i++)
            {
                Debug.Log("Start CreateEmptyHeart");
                CreateEmptyHeart();
                Debug.Log("End CreateEmptyHeart");
            }

            for (int i = 0; i < hearts.Count; i++)
            {
                int heartsStatusRemainder = (int)Mathf.Clamp(HUDManager.Instance.Health - (i * 2), 0, 2);
                Debug.Log(HUDManager.Instance.Health);
                hearts[i].SetHeartImage((HeartStatus)heartsStatusRemainder);
            } 

        }

        public void CreateEmptyHeart()
        {
            Debug.Log("CreateEmptyHeart");
            GameObject newHeart = Instantiate(heartPrefab);
            newHeart.transform.SetParent(transform);
            Debug.Log("CreateEmptyHeart2");
            HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
            heartComponent.SetHeartImage(HeartStatus.Empty);
            hearts.Add(heartComponent);
            Debug.Log("CreateEmptyHeart3");

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
