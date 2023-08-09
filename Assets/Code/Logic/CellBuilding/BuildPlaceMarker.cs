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
            Transform self = transform;
            _buildOffset = self.TransformVector(buildOffset);
            _location = new Location(self.position, self.rotation);
        }

        private void OnDrawGizmos()
        {
            Vector3 position = transform.position + _buildOffset;
            Gizmos.DrawWireCube(position, Vector3.one);
            Vector3 forwardMarkedPosition = position + transform.forward;
            Gizmos.DrawWireSphere( forwardMarkedPosition, 0.25f);
        }
    }
}