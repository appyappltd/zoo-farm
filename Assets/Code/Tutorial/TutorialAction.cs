using System;

namespace Tutorial
{
    public class TutorialAction : TutorialModule
    {
        private readonly Action _action;

        public TutorialAction(Action action)
        {
            _action = action;
        }

        public override void Play()
        {
            _action.Invoke();
            Complete();
        }
    }
}
