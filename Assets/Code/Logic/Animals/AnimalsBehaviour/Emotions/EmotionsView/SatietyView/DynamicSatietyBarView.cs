using Logic.SpriteUtils;
using StaticData;
using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour.Emotions.EmotionsView.SatietyView
{
    public class DynamicSatietyBarView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _background;
        [SerializeField] private SpriteRenderer _fill;
        [SerializeField] private SpriteMask _mask;
        [SerializeField] private SpriteFillMask _fillMask;

        public void Construct(SatietyIconsConfig iconsConfig)
        {
            _background.sprite = iconsConfig.SatietyBarBack;
            _fill.sprite = iconsConfig.SatietyBarFill;
            _mask.sprite = iconsConfig.Mask;
        }

        public void SetFill(float value)
        {
            _fillMask.SetFill(value);
        }
    }
}