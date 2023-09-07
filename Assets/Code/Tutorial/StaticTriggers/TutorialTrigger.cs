using System;

namespace Tutorial.StaticTriggers
{
    public class TutorialTrigger : ITutorialTrigger
    {
        public event Action Triggered = () => { };

        public void Trigger() =>
            Triggered.Invoke();
    }
}