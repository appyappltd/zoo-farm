using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour.Emotions
{
    public class EmotionView : MonoBehaviour
    {
        [field: SerializeField] public EmotionId EmotionId { get; set; }

        public void Show()
        {
            gameObject.SetActive( true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}