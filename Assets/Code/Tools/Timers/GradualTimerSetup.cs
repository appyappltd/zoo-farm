using UnityEngine;

namespace Code.Tools.Timers
{
    public struct GradualTimerSetup
    {
        public readonly float InitialDelay;
        public readonly float FinalDelay;
        public readonly float DelaySteps;
        public readonly AnimationCurve DelayCurve;

        public GradualTimerSetup(float initialDelay, float finalDelay, float delaySteps, AnimationCurve delayCurve)
        {
            InitialDelay = initialDelay;
            FinalDelay = finalDelay;
            DelaySteps = delaySteps;
            DelayCurve = delayCurve;
        }
    }
}