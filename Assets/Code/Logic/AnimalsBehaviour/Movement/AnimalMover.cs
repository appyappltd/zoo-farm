using NaughtyAttributes;
using Tools.Extension;
using UnityEngine;
using UnityEngine.AI;

namespace Logic.AnimalsBehaviour.Movement
{
    public class AnimalMover : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Vector3 _destination;

        private Vector3 _destinationPoint;

        public Vector3 DestinationPoint => _agent.destination;
        public float Distance => _agent.remainingDistance;
        public float StoppingDistance => _agent.stoppingDistance;

        public void SetDestination(Vector3 position)
        {
            _agent.SetDestination(position.ChangeY(transform.position.y));
        }

        [Button("SetDebugDestination")]
        private void SetDebugDestination()
        {
            SetDestination(_destination);
        }
    }
}