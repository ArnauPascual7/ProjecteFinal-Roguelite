using UnityEngine;

namespace Roguelite
{
    [CreateAssetMenu(fileName = "Item", menuName = "Item")]
    public class ItemObject : ScriptableObject
    {
        public int ID;
        public string DisplayName;
        [TextArea(4, 4)]
        public string Description;
    }
}