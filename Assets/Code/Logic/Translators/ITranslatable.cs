using System;

namespace Logic.Translators
{
    public interface ITranslatable
    {
        event Action<ITranslatable> Begin;
        event Action<ITranslatable> End;
        void Play();
        bool TryUpdate();
        void Enable();
        void Disable();
        bool IsPreload { get; }
    }
}