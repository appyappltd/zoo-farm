using NTC.Global.Cache.Interfaces;

namespace Tutorial
{
    public abstract class CutsceneModule : ICutsceneModule, IRunSystem
    {
        private ICutsceneModule _nextModule = default;

        public virtual void OnRun() { }

        public abstract void Play();

        public void Complete()
        {
            _nextModule?.Play();
        }

        public void AttachNext(ICutsceneModule next)
        {
            _nextModule = next;
        }
    }
}