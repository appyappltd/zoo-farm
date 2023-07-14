using NTC.Global.Cache;
using UnityEngine;
using UnityEngine.AI;

namespace Logic.Movement
{
    public class NavMeshMover : MonoCache
    {
        [SerializeField] private float _maxSpeed;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _rotateSpeed;

        public Vector3 DestinationPoint => _agent.destination;
        public float Distance => _agent.remainingDistance;
        public float StoppingDistance => _agent.stoppingDistance;
        public float NormalizedSpeed => _agent.speed / _maxSpeed;

        private void Start() =>
            _agent.speed = _maxSpeed;

        protected override void FixedRun() =>
            Rotate();

        public void SetNormalizedSpeed(float normalizedSpeed)
        {
            normalizedSpeed = Mathf.Clamp01(normalizedSpeed);
            _agent.speed = normalizedSpeed * _maxSpeed;
        }
        
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
            
            _agent.SetPath(path);
        }

        public void RotateTo(Vector3 target) =>
            transform.rotation = Quaternion.LookRotation(target - _agent.steeringTarget);

        private void Rotate()
        {
            Quaternion lookRotation = GetLookRotation();
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, lookRotation, _rotateSpeed * Time.fixedDeltaTime);
            transform.rotation = targetRotation;
        }

        private Quaternion GetLookRotation()
        {
            return Quaternion.LookRotation(_agent.steeringTarget - transform.position);
        }
    }
}