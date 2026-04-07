using Roguelite.Player;
using UnityEngine;

namespace Roguelite.Behaviours
{
    public class AimRotationBehaviour : MonoBehaviour
    {
        public float GetAngleTowardsMouse(Camera camera, Vector2 MousePosition)
        {
            Vector3 mousePos = camera.ScreenToWorldPoint(MousePosition);
            Vector3 mouseDirection = mousePos - transform.position;
            mouseDirection.z = 0f;

            float angle = (Vector3.SignedAngle(Vector3.right, mouseDirection, Vector3.forward) + 360f) % 360f;

            return angle;
        }
    }
}
