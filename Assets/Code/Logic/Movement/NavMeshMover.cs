using System.Diagnostics;
using NaughtyAttributes;
using NTC.Global.Cache;
using UnityEngine;
using UnityEngine.AI;
using Debug = Sisus.Debugging.Debug;

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

        public void Warp(Vector3 warpPoint) =>
            _agent.Warp(warpPoint);

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
            
            Debug.LogIf(_isAlignAtEnd == false, "You set the final rotation, but its application is not enabled");

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
                    
                    Debug.DrawRay(destination, Vector3.up * 50, Color.red, 10f);
                    
                    if ((pathEndZoneIndex & _areaMask) == 0)
                        return false;
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
            
            Debug.LogWarning("Path cannot be found");

            return false;
        }

        private void CheckForEndMove()
        {
            if (_agent.isStopped == false)
                return;
            
            Stop();

            if (_isAlignAtEnd)
                _aligner.Aligne(_finalRotation);
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