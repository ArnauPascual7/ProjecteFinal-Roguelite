using System;
using System.IO;
using UnityEngine;

namespace Roguelite.Systems
{
    public class SaveSystem
    {
        private static SaveData _saveData = new SaveData();

        [Serializable]
        public struct SaveData
        {
            public ObjectSaveData ObjectData;
        }

        public static string GetFilePath()
        {
            return Application.persistentDataPath + "/save" + ".save";
        }

        public static void Save()
        {
            HandleSaveData();

            File.WriteAllText(GetFilePath(), JsonUtility.ToJson(_saveData, true));
        }

        private static void HandleSaveData()
        {
            SaveObjectTemplate.Instance.Save(ref _saveData.ObjectData);
        }

        public static void Load()
        {
            string content = File.ReadAllText(GetFilePath());
            _saveData = JsonUtility.FromJson<SaveData>(content);

            HandleLoadData();
        }

        private static void HandleLoadData()
        {
            SaveObjectTemplate.Instance.Load(_saveData.ObjectData);
        }
    }
}
