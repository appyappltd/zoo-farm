using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour.Emotions.EmotionsView
{
    public class EmotionView : MonoBehaviour
    {
        public void Show() =>
            gameObject.SetActive(true);

        public void Hide() =>
            gameObject.SetActive(false);
    }
}