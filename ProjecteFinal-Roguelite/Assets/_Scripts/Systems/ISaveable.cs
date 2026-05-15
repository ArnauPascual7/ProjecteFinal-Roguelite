using System;
using System.Collections.Generic;
using UnityEngine;

namespace Roguelite.Systems
{
    public interface ISaveable
    {
        void PopulateSaveData(ref ObjectSaveData data);
        void LoadFromSaveData(ObjectSaveData data);
    }

    [Serializable]
    public struct ObjectSaveData
    {
        public int coins;
        public int playerLevel;
        public List<UpgradeSaveEntry> upgrades;
    }

    [Serializable]
    public class UpgradeSaveEntry
    {
        public string upgradeID;
        public int level;
    }
}
