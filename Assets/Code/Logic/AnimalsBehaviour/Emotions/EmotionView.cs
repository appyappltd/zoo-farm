using UnityEngine;

namespace Logic.AnimalsBehaviour.Emotions
{
    public class EmotionView : MonoBehaviour
    {
        [field: SerializeField] public EmotionId EmotionId { get; private set; }

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