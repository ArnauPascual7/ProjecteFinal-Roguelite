using Roguelite.Player;
using UnityEngine;

namespace Roguelite.Behaviours
{
    public class AimRotationBehaviour : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        public float GetAngleTowardsMouse(Vector2 MousePosition)
        {
            Vector3 mouseDirection = GetDirectionTowardsMouse(MousePosition);
            mouseDirection.z = 0f;

            float angle = (Vector3.SignedAngle(Vector3.right, mouseDirection, Vector3.forward) + 360f) % 360f;

            return angle;
        }

        public Vector3 GetDirectionTowardsMouse(Vector2 MousePosition)
            => _camera.ScreenToWorldPoint(MousePosition) - transform.position;
    }
}
