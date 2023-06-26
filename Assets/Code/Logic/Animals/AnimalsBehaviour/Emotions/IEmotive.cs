using System;

namespace Logic.Animals.AnimalsBehaviour.Emotions
{
    public interface IEmotive
    {
        event Action<EmotionId> ShowEmotion;
        event Action<EmotionId> SuppressEmotion;
    }
}