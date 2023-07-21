using Data;
using Data.ItemsData;
using Infrastructure.Factory;
using Services;
using StaticData;
using UnityEngine;

namespace Logic.Animals
{
    public class AnimalItemSpawner : MonoBehaviour
    {
        [SerializeField] private AnimalsSequenceConfig _sequenceConfig;

        private IHandItemFactory _handItemFactory;
        private int _currentPairIndex;

        private void Awake() =>
            _handItemFactory = AllServices.Container.Single<IGameFactory>().HandItemFactory;

        public HandItem SpawnAnimal(Vector3 at)
        {
            AnimalAndTreatToolPair pair = NextPair();
            HandItem spawnedAnimal = _handItemFactory.CreateAnimal(at, Quaternion.identity, pair.AnimalStaticData.AnimalType);
            spawnedAnimal.Construct(new AnimalItemData(pair.AnimalStaticData, pair.TreatTool));
            return spawnedAnimal;
        }

        private AnimalAndTreatToolPair NextPair()
        {
            if (_currentPairIndex >= _sequenceConfig.PairsCount - 1)
                _currentPairIndex = 0;

            AnimalAndTreatToolPair result = _sequenceConfig.GetPair(_currentPairIndex);
            _currentPairIndex++;
            return result;
        }
    }
}
