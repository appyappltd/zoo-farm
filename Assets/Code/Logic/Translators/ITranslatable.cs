using System;

namespace Logic.Translators
{
    public interface ITranslatable
    {
        event Action<ITranslatable> Begin;
        event Action<ITranslatable> End;
        void Play();
        void Stop(bool atCycleEnd);
        bool TryUpdate();
        void ResetToDefault();
        void Enable();
        void Disable();
        bool IsPreload { get; }
    }
}