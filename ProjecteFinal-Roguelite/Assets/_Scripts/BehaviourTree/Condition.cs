using UnityEngine;

namespace Roguelite.BehaviourTree
{
    public class Condition
    {
        public string cname;
        public bool check;

        public Condition(string name)
        {
            cname = name.ToString();
            check = false;
        }
    }
}
