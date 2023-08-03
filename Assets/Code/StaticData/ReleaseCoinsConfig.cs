using AYellowpaper.SerializedCollections;
using Logic.Animals;
using UnityEngine;

namespace StaticData
{
    public class ReleaseCoinsConfig : ScriptableObject
    {
        [SerializeField] private SerializedDictionary<AnimalId, int> _coinsPerAnimalRelease;

        public int Coins(AnimalId forAnimal) =>
            _coinsPerAnimalRelease[forAnimal];
    }
}