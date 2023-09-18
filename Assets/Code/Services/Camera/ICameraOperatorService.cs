using UnityEngine;

namespace Services.Camera
{
    public interface ICameraOperatorService : IService
    {
        void Focus(Transform followTarget);
        void Focus(Vector3 onPosition);
        void RegisterCamera(UnityEngine.Camera camera);
        void FocusOnDefault();
        void SetAsDefault(Transform target);
        Vector3 GetClosestRayPoint(Ray ray, float offset);
    }
}