using Data;
using Data.ItemsData;
using StaticData;
using UnityEngine;

namespace Logic.Animals
{
    public class AnimalSpawner : MonoBehaviour
    {
        [SerializeField] private HandItem _animal;
        [SerializeField] private AnimalsSequenceConfig _sequenceConfig;

        private int _currentPaidIndex;
        
        public HandItem SpawnAnimal(Vector3 at)
        {
            HandItem instantiateAnimal = Instantiate(_animal, at, Quaternion.identity);
            AnimalAndTreatToolPair pair = NextPair();
            instantiateAnimal.Construct(new AnimalItemData(pair.AnimalStaticData, pair.TreatTool));
            return instantiateAnimal;
        }

        private AnimalAndTreatToolPair NextPair()
        {
            if (_currentPaidIndex >= _sequenceConfig.PairsCount - 1)
            {
                _currentPaidIndex = 0;
            }
            
            AnimalAndTreatToolPair result = _sequenceConfig.GetPair(_currentPaidIndex);
            _currentPaidIndex++;
            return result;
        }
    }
}
