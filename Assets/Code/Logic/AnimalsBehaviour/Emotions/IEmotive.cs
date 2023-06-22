using System;

namespace Logic.AnimalsBehaviour.Emotions
{
    public interface IEmotive
    {
        event Action<EmotionId> ShowEmotion;
        event Action<EmotionId> SuppressEmotion;
    }
}