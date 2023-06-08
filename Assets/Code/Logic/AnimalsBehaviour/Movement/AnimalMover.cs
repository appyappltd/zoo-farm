using System.Collections.Generic;
using NTC.Global.Cache;
using Tools.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace Logic.AnimalsBehaviour.Movement
{
    public class AnimalMover : MonoCache
    {
        [SerializeField] private float _maxSpeed;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _rotateSpeed;

        private Vector3 _destinationPoint;
        private Vector3 _currentCorner;
        private Queue<Vector3> _corners = new Queue<Vector3>();

        public Vector3 DestinationPoint => _agent.destination;
        public float Distance => _agent.remainingDistance;
        public float StoppingDistance => _agent.stoppingDistance;
        public float NormalizedSpeed => _agent.speed / _maxSpeed;

        private void Start() =>
            _agent.speed = _maxSpeed;

        protected override void FixedRun() =>
            Rotate();

        public void SetDestination(Vector3 position)
        {
            NavMeshPath path = new NavMeshPath();

            if (_agent.CalculatePath(position, path))
            {
                for (int index = 1; index < path.corners.Length; index++)
                {
                    Vector3 cornerFrom = path.corners[index - 1];
                    Vector3 cornerTo = path.corners[index];

                    Debug.DrawLine(cornerFrom, cornerTo, Color.blue, 10f);
                }
            }

            foreach (Vector3 corner in path.corners)
                _corners.Enqueue(corner);

            _agent.SetPath(path);
        }

        private void Rotate()
        {
            Quaternion lookRotation = GetLookRotation();
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, lookRotation, _rotateSpeed * Time.fixedDeltaTime);

            transform.rotation = targetRotation;
        }

        private Quaternion GetLookRotation()
        {
            return Quaternion.LookRotation(_agent.velocity.ChangeY(_agent.transform.position.y));
        }
    }
}