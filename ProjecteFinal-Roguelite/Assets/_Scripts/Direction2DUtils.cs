using System;
using UnityEngine;

namespace Roguelite
{
    public static class Direction2DUtils
    {
        public static Direction GetOposite(Direction dir)
        {
            if (dir == Direction.Up) return Direction.Down;
            if (dir == Direction.Right) return Direction.Left;
            if (dir == Direction.Down) return Direction.Up;
            if (dir == Direction.Left) return Direction.Right;

            Debug.LogError($"DIRECTION 2D UTILS: Invalid direction: {dir}");
            return default;
        }

        public static Direction GetDirectionFromVector(Vector2Int vector)
        {
            if (vector == Vector2Int.up) return Direction.Up;
            if (vector == Vector2Int.right) return Direction.Right;
            if (vector == Vector2Int.down) return Direction.Down;
            if (vector == Vector2Int.left) return Direction.Left;

            Debug.LogError($"DIRECTION 2D UTILS: Invalid vector: {vector}");
            return default;
        }

        public static Vector2Int ToVector2Int(Direction dir)
        {
            if (dir == Direction.Up) return Vector2Int.up;
            if (dir == Direction.Right) return Vector2Int.right;
            if (dir == Direction.Down) return Vector2Int.down;
            if (dir == Direction.Left) return Vector2Int.left;

            Debug.LogError($"DIRECTION 2D UTILS: Invalid direction: {dir}");
            return default;
        }
    }

    [Serializable]
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
}
