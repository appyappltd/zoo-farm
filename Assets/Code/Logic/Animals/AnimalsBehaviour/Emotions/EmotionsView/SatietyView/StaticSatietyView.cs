using NTC.Global.System;
using Ui.Elements;
using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour.Emotions.EmotionsView.SatietyView
{
    public class StaticSatietyView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _sprite;
        [SerializeField] private TextSetter _coefficientText;

        public void Show(Sprite icon, int coefficient)
        {
            _sprite.sprite = icon;
            gameObject.SetActive(true);

            if (coefficient > 1)
            {
                _coefficientText.gameObject.Enable();
                _coefficientText.SetText($"x{coefficient}");
                return;
            }

            _coefficientText.gameObject.Disable();
        }
    }
}