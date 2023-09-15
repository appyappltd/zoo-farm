using System.Diagnostics;
using NaughtyAttributes;
using NTC.Global.Cache;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

namespace Logic.Movement
{
    public class NavMeshMover : MonoCache
    {
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _rotateSpeed;
        [SerializeField] private bool _isAlignAtEnd;
        [SerializeField] [ShowIf("_isAlignAtEnd")] private Aligner _aligner;
        [SerializeField] private int _areaMask;

        private Quaternion _finalRotation;

        public Vector3 DestinationPoint => _agent.destination;
        public float Distance => _agent.remainingDistance;
        public float StoppingDistance => _agent.stoppingDistance;
        public float NormalizedSpeed => _agent.speed / _maxSpeed;

        private void Awake()
        {
            _agent.speed = _maxSpeed;
        }

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

        public bool SetLocation(Location location)
        {
            _finalRotation = location.Rotation;

#if DEBUG
            if (_isAlignAtEnd == false)
            {
                Debug.LogWarning("You set the final rotation, but its application is not enabled");
            }

#endif
            return SetDestination(location.Position);
        }

        public bool SetDestination(Vector3 destination, bool isIgnoreLayerMask = false)
        {
            NavMeshPath path = new NavMeshPath();

            if (_agent.CalculatePath(destination, path))
            {
                if (isIgnoreLayerMask == false &&
                    NavMesh.SamplePosition(destination, out NavMeshHit endNavHit, 2f, NavMesh.AllAreas))
                {
                    int pathEndZoneIndex = endNavHit.mask;
#if UNITY_EDITOR
                    Debug.Log("Индекс зоны назначения: " + pathEndZoneIndex);
                    Debug.DrawRay(destination, Vector3.up * 50, Color.red, 10f);
#endif
                    if ((pathEndZoneIndex & _areaMask) == 0)
                    {
#if DEBUG
                        Debug.LogWarning("Destination is not on valid area layer");
#endif
                        return false;
                    }
                }

#if UNITY_EDITOR
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
                return true;
            }


            // SetDestination(destination);
#if DEBUG
            Debug.LogWarning("Path cannot be found");
#endif

            return false;
        }

        private void Rotate()
        {
            Quaternion lookRotation = GetLookRotation();
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, lookRotation, _rotateSpeed * Time.smoothDeltaTime);
            transform.rotation = targetRotation;
        }

        private Quaternion GetLookRotation() =>
            Quaternion.LookRotation(_agent.steeringTarget - transform.position);

        [Button]
        [Conditional("UNITY_EDITOR")]
        private void ReadAgentMask() =>
            _areaMask = _agent.areaMask;
    }
}