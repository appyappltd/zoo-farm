using AYellowpaper.SerializedCollections;
using Logic.Animals;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Release Coins Config", fileName = "NewReleaseCoinsConfig", order = 0)]
    public class ReleaseCoinsConfig : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<AnimalType, int> _coinsPerAnimalRelease;

        public int Coins(AnimalType forAnimal) =>
            _coinsPerAnimalRelease[forAnimal];
    }
}