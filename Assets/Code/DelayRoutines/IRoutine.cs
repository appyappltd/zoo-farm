namespace DelayRoutines
{
    public interface IRoutine
    {
        public bool IsActive { get; }
        public void AddNext(IRoutine routine);
        public void Play();
        void Stop();
    }
}