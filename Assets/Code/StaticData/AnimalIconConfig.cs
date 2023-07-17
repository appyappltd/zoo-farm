using AYellowpaper.SerializedCollections;
using Logic.Animals;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Animal Icon Config", fileName = "NewAnimalIconConfigs", order = 0)]
    public class AnimalIconConfig : ScriptableObject
    {
        public SerializedDictionary<AnimalType, Sprite> AnimalIcons;
    }
}