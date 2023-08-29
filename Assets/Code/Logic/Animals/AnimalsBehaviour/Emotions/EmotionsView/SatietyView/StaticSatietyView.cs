using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour.Emotions.EmotionsView.SatietyView
{
    public class StaticSatietyView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;

        public void Show(Sprite icon)
        {
            _sprite.sprite = icon;
        }
    }
}