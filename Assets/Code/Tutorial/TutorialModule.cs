using NTC.Global.Cache.Interfaces;

namespace Tutorial
{
    public abstract class TutorialModule : ITutorialModule, IRunSystem
    {
        private ITutorialModule _nextModule = default;

        public virtual void OnRun() { }

        public abstract void Play();

        public void Complete()
        {
            _nextModule?.Play();
        }

        public void AttachNext(ITutorialModule next)
        {
            _nextModule = next;
        }
    }
}