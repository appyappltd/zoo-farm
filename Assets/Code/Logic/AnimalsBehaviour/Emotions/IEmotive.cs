using System;

namespace Logic.AnimalsBehaviour.Emotions
{
    public interface IEmotive
    {
        event Action<Emotions> ShowEmotion;
        event Action<Emotions> SuppressEmotion;
    }
}