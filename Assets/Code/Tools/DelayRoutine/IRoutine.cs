namespace Tools.DelayRoutine
{
    public interface IRoutine
    {
        public void AddNext(IRoutine routine);
        public void Play();
    }
}