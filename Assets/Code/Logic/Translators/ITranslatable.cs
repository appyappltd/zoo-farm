using System;

namespace Logic.Translators
{
    public interface ITranslatable
    {
        event Action<ITranslatable> Begin;
        event Action<ITranslatable> End;
        bool TryUpdate();
        void Enable();
        void Disable();
    }
}