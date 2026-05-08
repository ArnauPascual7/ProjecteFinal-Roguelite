using System;
using UnityEngine;

namespace Roguelite.Systems
{
    public class SaveObjectTemplate : MonoBehaviour
    {
        #region Instance (Only used for access)
        public static SaveObjectTemplate Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        #endregion

        public void Save(ref ObjectSaveData data)
        {
            data.name = "HOLA";
            data.num = 33;
        }

        public void Load(ObjectSaveData data)
        {
            Debug.Log($"Name: {data.name}, Num: {data.num}");
        }
    }

    [Serializable]
    public struct ObjectSaveData
    {
        public string name;
        public int num;
    }
}
