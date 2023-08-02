using System;

namespace DelayRoutines
{
    public class LoopWhile : Loop
    {
        private readonly Func<bool> _repeatCondition;

        public LoopWhile(Func<bool> repeatCondition)
        {
            _repeatCondition = repeatCondition;
        }

        protected override void OnPlay()
        {
            if (_repeatCondition.Invoke())
            {
                Iterate();
                return;
            }

            Next();
        }
    }
}