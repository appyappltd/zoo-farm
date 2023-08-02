using NaughtyAttributes;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Presets/Satiety Icons Config", fileName = "NewSatietyIconsConfig", order = 0)]
    public class SatietyIconsConfig : ScriptableObject
    {
        [ShowAssetPreview] public Sprite FullSatiety;
        [ShowAssetPreview] public Sprite SatietyBarBack;
        [ShowAssetPreview] public Sprite SatietyBarFill;
        [ShowAssetPreview] public Sprite EmptySatiety;
        [ShowAssetPreview] public Sprite Mask;
    }
}