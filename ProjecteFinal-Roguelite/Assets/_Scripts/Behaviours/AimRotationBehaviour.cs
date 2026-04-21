using Roguelite.Player;
using UnityEngine;

namespace Roguelite.Behaviours
{
    public class AimRotationBehaviour : MonoBehaviour
    {
        public float GetAngleTowardsMouse(Camera camera, Vector2 MousePosition)
        {
            Vector3 mouseDirection = GetDirectionTowardsMouse(camera, MousePosition);
            mouseDirection.z = 0f;

            float angle = (Vector3.SignedAngle(Vector3.right, mouseDirection, Vector3.forward) + 360f) % 360f;

            return angle;
        }

        public Vector3 GetDirectionTowardsMouse(Camera camera, Vector2 MousePosition)
            => camera.ScreenToWorldPoint(MousePosition) - transform.position;
    }
}
