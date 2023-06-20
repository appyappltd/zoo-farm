using Cameras;
using NaughtyAttributes;
using Services;
using Services.Camera;
using UnityEngine;

namespace Logic
{
    public class TestCameraOperator : MonoBehaviour
    {
        [SerializeField] private CameraMovement _camera;
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _position;
        
        private ICameraOperatorService _cameraService;

        private void Awake()
        {
            _cameraService = AllServices.Container.Single<ICameraOperatorService>();
            _cameraService.RegisterCamera(_camera);
        }

        [Button("Follow", EButtonEnableMode.Playmode)]
        private void Follow()
        {
            _cameraService.Focus(_target);
        }
        
        [Button("Move", EButtonEnableMode.Playmode)]
        private void Move()
        {
            _cameraService.Focus(_position);
        }
    }
}