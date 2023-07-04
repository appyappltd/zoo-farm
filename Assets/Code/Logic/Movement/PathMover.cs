using UnityEngine;

namespace Logic.Movement
{
    public class PathMover : MonoBehaviour
    {
        [SerializeField] private Transform[] _points;

        private int index = 0;
        private IItemMover _itemMover;
        private Rotator _rotator;
        private bool _isPath = false;

        private void Awake()
        {
            _rotator = GetComponent<Rotator>();
            _itemMover = GetComponent<IItemMover>();
            _itemMover.Ended += SetPoint;
        }

        public void StartWalk()
        {
            _isPath = true;
            Walk();
        }

        public void SetPoints(Transform[] points) => _points = points;

        public void Walk()
        {
            var point = _points[index];
            _rotator.Rotate(point);
            _itemMover.Move(point);
        }

        private void SetPoint()
        {
            if (_isPath && index + 1 < _points.Length)
            {
                index++;
                Walk();
            }
        }
    }
}
