namespace Cutscene
{
    public class CutsceneTriggerAwaiter : CutsceneModule
    {
        private readonly ICutsceneTrigger _trigger;

        public CutsceneTriggerAwaiter(ICutsceneTrigger trigger)
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