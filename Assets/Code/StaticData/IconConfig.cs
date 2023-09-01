using AYellowpaper.SerializedCollections;
using Logic.Animals;
using Logic.Foods.FoodSettings;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Static Data/ Icons Config", fileName = "NewIconsConfig", order = 0)]
    public class IconConfig : ScriptableObject
    {
        public SerializedDictionary<AnimalType, Sprite> AnimalIcons;
        public SerializedDictionary<FoodId, Sprite> FoodIcons;
    }
}