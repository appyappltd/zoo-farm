using UnityEngine;

namespace Logic.AnimalsBehaviour.Emotions
{
    public struct Emotion
    {
        public readonly Sprite Sprite;
        public readonly Emotions Name;

        public Emotion(Emotions name, Sprite sprite)
        {
            Name = name;
            Sprite = sprite;
        }
    }
}