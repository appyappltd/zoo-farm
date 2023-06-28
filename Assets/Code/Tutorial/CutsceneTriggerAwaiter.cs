namespace Tutorial
{
    public class CutsceneTriggerAwaiter : CutsceneModule
    {
        private readonly ITutorialTrigger _trigger;

        public CutsceneTriggerAwaiter(ITutorialTrigger trigger)
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