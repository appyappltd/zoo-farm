using System.Collections.Generic;
using UnityEngine;

namespace Logic.AnimalsBehaviour.Emotions
{
    public class EmotionProvider : MonoBehaviour
    {
        private GameObject _currentEmotion;

        // Заменить на сериализуемый словарь
        private Dictionary<EmotionId, GameObject> _emotions = new Dictionary<EmotionId, GameObject>();

        public void ChangeEmotion(EmotionId id)
        {
            _currentEmotion.SetActive(false);
            _currentEmotion = _emotions[id];
            _currentEmotion.SetActive(true);
        }
    }
}