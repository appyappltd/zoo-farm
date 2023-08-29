using NaughtyAttributes;
using NTC.Global.Cache;
using UnityEngine;
using UnityEngine.AI;

namespace Logic.Movement
{
    public class NavMeshMover : MonoCache
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private bool _isAlignAtEnd;
        [SerializeField] [ShowIf("_isAlignAtEnd")] private Aligner _aligner;

        private Quaternion _finalRotation;

        public Vector3 DestinationPoint => _agent.destination;
        public float Distance => _agent.remainingDistance;
        public float StoppingDistance => _agent.stoppingDistance;
        public float NormalizedSpeed => _agent.speed / _maxSpeed;

        private void Start() =>
            _agent.speed = _maxSpeed;

        protected override void Run()
        {
            Rotate();
            CheckForEndMove();
        }

        private void CheckForEndMove()
        {
            if (_agent.isStopped == false)
                return;
            
            Stop();

            if (_isAlignAtEnd)
                _aligner.Aligne(_finalRotation);
        }

        public void Stop()
        {
            enabled = false;
            _agent.isStopped = true;
        }
        
        public void ActivateAlignAtEnd() =>
            _isAlignAtEnd = true;
        
        public void DeactivateAlignAtEnd() =>
            _isAlignAtEnd = false;

        public void SetNormalizedSpeed(float normalizedSpeed)
        {
            normalizedSpeed = Mathf.Clamp01(normalizedSpeed);
            _agent.speed = normalizedSpeed * _maxSpeed;
        }

        public void SetLocation(Location location)
        {
            SetDestination(location.Position);
            _finalRotation = location.Rotation;

#if DEBUG
            if (_isAlignAtEnd == false)
            {
                Debug.LogWarning("You set the final rotation, but its application is not enabled");
            }
#endif
        }

        public void SetDestination(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();

            if (_agent.CalculatePath(destination, path))
            {
#if DEBUG
                for (int index = 1; index < path.corners.Length; index++)
                {
                    Vector3 cornerFrom = path.corners[index - 1];
                    Vector3 cornerTo = path.corners[index];

                    Debug.DrawLine(cornerFrom, cornerTo, Color.blue, 10f);
                }
#endif
                enabled = true;
                _agent.isStopped = false;
                _agent.SetPath(path);
                return;
            }

            // SetDestination(destination);
#if DEBUG
            Debug.LogWarning("Path cannot be found");
#endif
        }

        private void Rotate()
        {
            Quaternion lookRotation = GetLookRotation();
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, lookRotation, _rotateSpeed * Time.smoothDeltaTime);
            transform.rotation = targetRotation;
        }

        private Quaternion GetLookRotation() =>
            Quaternion.LookRotation(_agent.steeringTarget - transform.position);
    }
}