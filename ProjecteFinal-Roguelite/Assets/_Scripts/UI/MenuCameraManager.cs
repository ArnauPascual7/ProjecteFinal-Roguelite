using UnityEngine;
using Unity.Cinemachine;

namespace Roguelite.camera
{
    public class MenuCameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera[] virtualCameras;
        [SerializeField] private CinemachineCamera startingCamera;

        private void Awake()
        {
            if (startingCamera != null)
            {
                SnapToCamera(startingCamera);
            }
        }

        private void Start()
        {
            if (virtualCameras == null || virtualCameras.Length == 0)
            {
                ValidateCameras();
            }

            if (startingCamera != null)
            {
                ActivateCamera(startingCamera);
            }
        }

        public void ActivateCamera(CinemachineCamera targetCamera)
        {
            if (virtualCameras == null || virtualCameras.Length == 0) ValidateCameras();

            foreach (var vcam in virtualCameras)
            {
                if (vcam != null)
                {
                    vcam.Priority = (vcam == targetCamera) ? 20 : 10;
                }
            }
        }

        // Moure la càmera instantàniament
        private void SnapToCamera(CinemachineCamera targetCamera)
        {
            // Buscar Main Camera
            Camera mainCam = Camera.main;
            if (mainCam != null && targetCamera != null)
            {
                // Prioritat alta ja d'entrada
                targetCamera.Priority = 20;

                // Moure càmera peincipal a la posició de la càmera virtual
                mainCam.transform.position = targetCamera.transform.position;

                var brain = mainCam.GetComponent<CinemachineBrain>();
                if (brain != null)
                {
                    // Cancel·lar qualsevol transició que estigués a punt de començar
                    brain.ActiveBlend = null;
                }
            }
        }

        private void ValidateCameras()
        {
            if (virtualCameras == null || virtualCameras.Length == 0)
            {
                virtualCameras = Object.FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None);
            }
        }
    }
}
