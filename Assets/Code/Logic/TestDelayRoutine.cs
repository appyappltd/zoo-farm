using NaughtyAttributes;
using Tools.DelayRoutine;
using UnityEngine;

namespace Logic
{
    public class TestDelayRoutine : MonoBehaviour
    {
        private DelayRoutine _delayRoutine;

        private void Awake()
        {
            _delayRoutine = new DelayRoutine();

            _delayRoutine
                .Then(() => Debug.Log("Begin"))
                .Wait(1f)
                .Then(() => Debug.Log("Wait for 1 sec"))
                .Wait(2f)
                .Then(() => Debug.Log("Wait for 2 sec"))
                .LoopFor(3);
        }

        [Button]
        private void Play()
        {
            _delayRoutine.Play();
        }

        [Button]
        private void Stop()
        {
            _delayRoutine.Play();
        }
    }
}