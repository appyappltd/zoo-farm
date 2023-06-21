using Logic.AnimalsBehaviour.Emotions;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "EmotionConfig", menuName = "Static Data/Emotion configs")]
    public class EmotionConfig : ScriptableObject
    {
        public EmotionId Name;
        public Sprite Sprite;
    }
}