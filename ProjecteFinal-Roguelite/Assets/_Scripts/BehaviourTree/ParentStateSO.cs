using System.Collections.Generic;
using UnityEngine;

namespace Roguelite.BehaviourTree
{
    [CreateAssetMenu(fileName = "_ParentState", menuName = "Behaviour Tree/Parent State")]
    public class ParentStateSO : ScriptableObject
    {
        public List<BehaviourState> states = new List<BehaviourState>();
    }
}
