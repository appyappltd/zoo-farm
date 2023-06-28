using System;

namespace Tutorial
{
    public class CutsceneAction : CutsceneModule
    {
        private readonly Action _action;

        public CutsceneAction(Action action)
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
