using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour.Emotions
{
    public struct Emotion
    {
        public readonly Sprite Sprite;
        public readonly EmotionId Name;

        public Emotion(EmotionId name, Sprite sprite)
        {
            Name = name;
            Sprite = sprite;
        }
    }
}