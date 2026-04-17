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
            // IMPORTANT: Fem el teletransport a l'Awake per anar per davant del render
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

        // Aquesta funció mou la càmera principal instantàniament
        private void SnapToCamera(CinemachineCamera targetCamera)
        {
            // Busquem la Main Camera
            Camera mainCam = Camera.main;
            if (mainCam != null && targetCamera != null)
            {
                // Posem la prioritat alta ja d'entrada
                targetCamera.Priority = 20;

                // Movem la posició de la càmera física exactament on està la virtual
                mainCam.transform.position = targetCamera.transform.position;

                // Avisem a Cinemachine que hem mogut la càmera a la força (Warp)
                // per evitar que intenti fer una transició suau al primer frame.
                var brain = mainCam.GetComponent<CinemachineBrain>();
                if (brain != null)
                {
                    // Això cancel·la qualsevol transició que estigués a punt de començar
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
