using System.Collections;
using UnityEngine;

namespace Roguelite.camera
{
    public class MenuCameraController : MonoBehaviour
    {
        [SerializeField] private float transitionSpeed = 5f;
        private Coroutine _moveCoroutine;

        public void MoveToPosition(Transform target)
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
            _moveCoroutine = StartCoroutine(MoveRoutine(target.position));
        }

        private IEnumerator MoveRoutine(Vector3 targetPos)
        {
            // Mentre no estigui molt aprop del punt de destí
            while (Vector3.Distance(transform.position, targetPos) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * transitionSpeed);
                yield return null;
            }
            transform.position = targetPos;
        }
    }
}

