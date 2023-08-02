using DelayRoutines;
using NaughtyAttributes;
using NTC.Global.Cache;
using UnityEngine;

namespace Logic
{
    public class TestDelayRoutine : MonoBehaviour
    {
        private DelayRoutine _delayRoutine;

        private bool _isPlay = true;

        private void Awake()
        {
            _delayRoutine = new DelayRoutine();

            Awaiter waitFor = new Awaiter(1f, GlobalUpdate.Instance);
            Executor logWait = new Executor(() => Debug.Log("Wait for 1 sec"));
            LoopWhile loopFor = new LoopWhile(RepeatCondition);

            _delayRoutine.Wait(waitFor)
                .Then(logWait)
                .LoopWhile(loopFor);

            _delayRoutine
                .Then(() => Debug.Log("Begin"))
                .Wait(1f)
                .Then(() => Debug.Log("Wait for 1 sec"))
                .Wait(2f)
                .Then(() => Debug.Log("Wait for 2 sec"))
                .LoopFor(3, 3)
                .Wait(2f)
                .Then(() => Debug.Log("Last wait for 2 sec"));
        }

        private bool RepeatCondition()
        {
            return _isPlay;
        }

        [Button]
        private void Play()
        {
            _delayRoutine.Play();
        }

        [Button]
        private void Stop()
        {
            _isPlay = false;
        }
    }
}