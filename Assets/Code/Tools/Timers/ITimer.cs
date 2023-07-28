namespace Tools.Timers
{
    public interface ITimer
    {
        bool IsActive { get; }
        void Reset();
        void Restart();
        void Tick(float delta);
    }
}