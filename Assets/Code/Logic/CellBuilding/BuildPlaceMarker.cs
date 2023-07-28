using UnityEngine;

namespace Logic.CellBuilding
{
    public class BuildPlaceMarker : MonoBehaviour
    {
        [SerializeField] private Sprite _icon;

        private Location _location;
        private Vector3 _buildOffset;
        
        public Location Location => _location;
        public Sprite Icon => _icon;
        public Vector3 BuildPosition => _location.Position + _buildOffset;

        public void Init(Vector3 buildOffset)
        {
            _buildOffset = transform.TransformVector(buildOffset);
            _location = new Location(transform.position, transform.rotation);
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawCube(transform.position, Vector3.one);
            Vector3 forwardMarkedPosition = transform.position + transform.forward;
            Gizmos.DrawSphere( forwardMarkedPosition, 0.25f);
        }
    }
}