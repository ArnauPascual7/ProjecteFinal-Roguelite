using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

namespace Roguelite.Systems
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null) 
            { 
                Destroy(gameObject); 
                return; 
            }
            Instance = this;
        }

        public void SaveAll()
        {
            // Crear el paquet gran (SaveData) i el contingut (ObjectData)
            SaveSystem.SaveData wrapper = new SaveSystem.SaveData();
            wrapper.ObjectData = new ObjectSaveData();
            wrapper.ObjectData.upgrades = new List<UpgradeSaveEntry>();

            // Buscar tots els ISaveable
            var saveables = Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Exclude).OfType<ISaveable>();

            foreach (var s in saveables)
            {
                s.PopulateSaveData(ref wrapper.ObjectData);
            }

            // Gruardar el paquet gran a JSON
            string json = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(SaveSystem.GetFilePath(), json);
            Debug.Log("Partida Guardada!");
        }

        public void LoadAll()
        {
            string path = SaveSystem.GetFilePath();
            if (!File.Exists(path)) return;

            // 1. Llegim el paquet gran
            string content = File.ReadAllText(path);
            SaveSystem.SaveData wrapper = JsonUtility.FromJson<SaveSystem.SaveData>(content);

            // 2. Repartim l'ObjectData intern als scripts
            var saveables = Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Exclude).OfType<ISaveable>();

            foreach (var s in saveables)
            {
                s.LoadFromSaveData(wrapper.ObjectData);
            }
            Debug.Log("Partida Carregada!");
        }
    }
}
