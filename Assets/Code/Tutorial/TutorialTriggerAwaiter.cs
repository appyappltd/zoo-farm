namespace Tutorial
{
    public class TutorialTriggerAwaiter : TutorialModule
    {
        private readonly ITutorialTrigger _trigger;

        public TutorialTriggerAwaiter(ITutorialTrigger trigger)
        {
            _trigger = trigger;
        }

        public override void Play()
        {
            _trigger.Triggered += OnTriggered;
        }

        private void OnTriggered()
        {
            Complete();
            _trigger.Triggered -= OnTriggered;
        }
    }
}