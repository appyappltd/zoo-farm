namespace Code.Tools.Timers
{
    public interface ITimer
    {
        bool IsActive { get; }
        void Reset();
        void Tick(float delta);
    }
}