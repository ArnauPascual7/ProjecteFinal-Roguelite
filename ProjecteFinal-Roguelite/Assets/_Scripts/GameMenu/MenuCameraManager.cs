using UnityEngine;
using Unity.Cinemachine;
using System;

namespace Roguelite.GameMenu
{
    public class MenuCameraManager : MonoBehaviour
    {
        [SerializeField] private CinemachineBrain _cinemachineBrain;
        [SerializeField] private VirtualCameraName _startingCamera;
        [SerializeField] private VirtualCamera[] _virtualCameras;
        [SerializeField] private float _defaultBlend = 1.5f;

        public static MenuCameraManager Instance { get; private set; }
        public VirtualCameraName StartingCameraName => _startingCamera;
        public float DefaultBlend => _defaultBlend;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _cinemachineBrain.DefaultBlend.Time = _defaultBlend;

            SnapToCamera(_startingCamera);
        }

        private void Start()
        {
            if (_virtualCameras == null || _virtualCameras.Length == 0)
            {
                Debug.LogError("MENU CAMERA MANAGER: No virtual cameras detected");
                return;
            }

            ActivateCamera(_startingCamera);
        }

        public void ActivateCamera(CinemachineCamera targetCamera)
        {
            if (_virtualCameras == null || _virtualCameras.Length == 0)
            {
                Debug.LogError("MENU CAMERA MANAGER: No virtual cameras detected");
                return;
            }

            foreach (var vcam in _virtualCameras)
            {
                CinemachineCamera cam = vcam.ccam;

                if (cam != null)
                {
                    cam.Priority = (cam == targetCamera) ? 20 : 10;
                }
            }
        }

        public void ActivateCamera(VirtualCameraName targetCamera)
        {
            if (_virtualCameras == null || _virtualCameras.Length == 0)
            {
                Debug.LogError("MENU CAMERA MANAGER: No virtual cameras detected");
                return;
            }

            foreach (var vcam in _virtualCameras)
            {
                CinemachineCamera ccam = vcam.ccam;

                if (ccam != null)
                {
                    ccam.Priority = (vcam.name == targetCamera) ? 20 : 10;
                }
            }
        }

        private void SnapToCamera(VirtualCameraName targetCamera)
        {
            if (_cinemachineBrain == null)
            {
                Debug.LogError("MENU CAMERA MANAGER: No main camera attached");
                return;
            }

            CinemachineCamera ccam = GetCCameraByName(targetCamera);

            if (ccam == null)
            {
                Debug.LogError("MENU CAMERA MANAGER: targetCamera does not exist");
                return;
            }

            // Prioritat alta ja d'entrada
            ccam.Priority = 20;

            // Moure càmera principal a la posició de la càmera virtual
            _cinemachineBrain.transform.position = ccam.transform.position;

            // Cancel·lar qualsevol transició que estigués a punt de començar
            _cinemachineBrain.ActiveBlend = null;
        }

        private CinemachineCamera GetCCameraByName(VirtualCameraName vcameraEnum)
        {
            foreach (var vcam in _virtualCameras)
            {
                if (vcam.name == vcameraEnum)
                {
                    return vcam.ccam;
                }
            }

            return null;
        }
    }

    public enum VirtualCameraName
    {
        StarterInitial,
        Items,
        Settings,
        Build,
        Upgrades
    }

    [Serializable]
    public struct VirtualCamera
    {
        public VirtualCameraName name;
        public CinemachineCamera ccam;
    }
}
