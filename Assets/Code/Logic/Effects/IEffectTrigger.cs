using System;

namespace Logic.Effects
{
    public interface IEffectTrigger
    {
        event Action EffectTriggered;
    }
}