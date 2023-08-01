namespace Tools.DelayRoutine
{
    public class Loop : Routine
    {
        private int _remainingTimes;

        public Loop(int times)
        {
            _remainingTimes = times;
        }

        public override void Play()
        {
            _remainingTimes--;

            if (_remainingTimes > 0)
            {
                Next();
            }
        }
    }
}