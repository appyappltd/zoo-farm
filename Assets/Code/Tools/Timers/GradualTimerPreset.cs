using UnityEngine;

namespace Code.Tools.Timers
{
    public class GradualTimerPreset : ScriptableObject
    {
        [SerializeField] private float _initialDelay;
        [SerializeField] private float _finalDelay;
        [SerializeField] private float _delaySteps;
        [SerializeField] private AnimationCurve _delayCurve;

        public GradualTimerSetup GetSetup() =>
            new GradualTimerSetup(_initialDelay, _finalDelay, _delaySteps, _delayCurve);
    }
}