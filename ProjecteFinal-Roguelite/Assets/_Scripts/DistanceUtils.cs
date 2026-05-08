using UnityEngine;

namespace Roguelite.Helpers
{
    public static class DistanceUtils
    {
        public static bool HasLineOfSight(Vector2 from, Vector2 to, float range, LayerMask ignoreLayers)
        {
            if (!IsInRange(from, to, range)) return false;
            
            Vector2 direction = GetDirection(from, to);
            float distance = GetDistance(from, to);
            bool hit = Physics2D.Raycast(from, direction, distance, ~ignoreLayers);

            Debug.DrawRay(from, direction * distance, hit ? Color.red : Color.green);

            return !hit;
        }

        public static bool IsInRange(Vector2 from, Vector2 to, float range)
            => GetDistance(from, to) <= range;

        public static float GetDistance(Vector2 from, Vector2 to)
            => Vector2.Distance(from, to);

        public static Vector2 GetDirection(Vector2 from, Vector2 to)
            => (to - from).normalized;
    }
}