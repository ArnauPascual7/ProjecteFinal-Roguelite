using UnityEngine;

namespace Roguelite
{
    public class RotationCorrectionBehaviuor : MonoBehaviour
    {
        [SerializeField] private float _offset = 90f;

        private void Start()
        {
            transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + _offset);
        }
    }
}
