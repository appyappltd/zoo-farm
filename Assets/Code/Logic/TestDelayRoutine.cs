using DelayRoutines;
using NaughtyAttributes;
using UnityEngine;

namespace Logic
{
    public class TestDelayRoutine : MonoBehaviour
    {
        private DelayRoutine _untilRoutine;

        private bool _isPlay = true;
        [SerializeField] private bool _isUntil = false;
        [SerializeField] private bool _isWhile = false;
        private DelayRoutine _whileRoutine;

        private void Awake()
        {
            _untilRoutine = new DelayRoutine();
            _untilRoutine.Then(() => Debug.Log("Begin Until")).WaitUntil(() => _isUntil).Then((() => Debug.Log("Until")));

            
            _whileRoutine = new DelayRoutine();
            _whileRoutine.Then(() => Debug.Log("Begin While")).WaitWhile(() => _isWhile).Then((() => Debug.Log("While")));
        }

        private bool RepeatCondition()
        {
            return _isPlay;
        }

        [Button]
        private void PlayUntil()
        {
            _untilRoutine.Play();
        }

        [Button] private void PlayWhile()
        {
            _whileRoutine.Play();
        }
        
        [Button]
        private void Stop()
        {
            _isPlay = false;
        }
    }
}