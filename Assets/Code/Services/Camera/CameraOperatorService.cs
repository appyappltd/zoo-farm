using Cameras;
using Logic.Cameras;
using UnityEngine;

namespace Services.Camera
{
    public class CameraOperatorService : ICameraOperatorService
    {
        private CameraMovement _mover;
        private Transform _defaultFollowTarget;
        private CameraFrame _frame;

        public void FocusOnDefault() =>
            Focus(_defaultFollowTarget);

        public void Focus(Transform followTarget) =>
            _mover.Follow(followTarget);

        public void Focus(Vector3 onPosition) =>
            _mover.MoveTo(onPosition);

        public void SetAsDefault(Transform target) =>
            _defaultFollowTarget = target;

        public void RegisterCamera(UnityEngine.Camera camera)
        {
            _mover = camera.GetComponent<CameraMovement>();
            _frame = new CameraFrame(camera);
        }

        public Vector3 GetClosestRayPoint(Ray ray, float offset) =>
            _frame.GetClosestRayPoint(ray, offset);
    }
}