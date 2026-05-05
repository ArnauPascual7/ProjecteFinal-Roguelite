using Roguelite.Player;
using UnityEngine;

namespace Roguelite.Behaviours
{
    public class AimRotationBehaviour : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public float GetAngleTowardsMouse(Vector2 mousePosition)
        {
            Vector3 mouseDirection = GetDirectionTowardsMouse(mousePosition);
            mouseDirection.z = 0f;

            float angle = (Vector3.SignedAngle(Vector3.right, mouseDirection, Vector3.forward) + 360f) % 360f;

            return angle;
        }

        public float GetAngleTowardsTarget(Vector2 targetPosition)
        {
            Vector2 targetDirection = GetDirectionTowardsTarget(targetPosition);

            float angle = (Vector3.SignedAngle(Vector3.right, targetDirection, Vector3.forward) + 360f) % 360f;

            return angle;
        }

        public Vector3 GetDirectionTowardsMouse(Vector2 mousePosition)
            => _camera.ScreenToWorldPoint(mousePosition) - transform.position;

        public Vector2 GetDirectionTowardsTarget(Vector2 targetPosition)
            => targetPosition - (Vector2)transform.position;
    }
}
