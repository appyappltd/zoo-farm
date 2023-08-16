using System;
using DelayRoutines;
using NaughtyAttributes;
using UnityEngine;

namespace Logic
{
    public class TestDelayRoutine : MonoBehaviour
    {
        private RoutineSequence _untilRoutineSequence;

        private bool _isPlay = true;
        [SerializeField] private bool _isUntil = false;
        [SerializeField] private bool _isWhile = false;
        private RoutineSequence _whileRoutineSequence;

        private void OnDestroy()
        {
            _whileRoutineSequence.Kill();
        }

        private void Awake()
        {
            _untilRoutineSequence = new RoutineSequence();
            _untilRoutineSequence.Then(() => Debug.Log("Begin Until")).WaitUntil(() => _isUntil).Then((() => Debug.Log("Until")));

            
            _whileRoutineSequence = new RoutineSequence();
            _whileRoutineSequence.Then(() => Debug.Log("Begin While")).WaitWhile(() => _isWhile).Then((() => Debug.Log("While")));
        }

        private bool RepeatCondition()
        {
            return _isPlay;
        }

        [Button]
        private void PlayUntil()
        {
            _untilRoutineSequence.Play();
        }

        [Button] private void PlayWhile()
        {
            _whileRoutineSequence.Play();
        }
        
        [Button]
        private void Stop()
        {
            _isPlay = false;
        }
    }
}