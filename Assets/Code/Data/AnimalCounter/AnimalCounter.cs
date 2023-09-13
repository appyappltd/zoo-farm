using System;
using System.Collections.Generic;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Services;
using Services.Animals;
using Tools.Global;

namespace Data.AnimalCounter
{
    public class AnimalCounter : IAnimalCounter, IDisposable
    {
        private readonly Dictionary<AnimalType, AnimalCountData> _countDatas = new Dictionary<AnimalType, AnimalCountData>();
        private readonly Dictionary<AnimalId, AnimalSatietyObserver> _satietyObservers = new Dictionary<AnimalId, AnimalSatietyObserver>();
        private readonly List<AnimalType> _availableTypes = new List<AnimalType>();
        private readonly IGlobalSettings _globalSettings;

        public event Action<AnimalType, AnimalCountData> Updated = (_, _) => { };

        public AnimalCounter()
        {
            _globalSettings = AllServices.Container.Single<IGlobalSettings>();
        }

        public int TotalBreedingReadyPairsCount => GetTotalReadyPairsCount();

        public void Register(IAnimal animal)
        {
            AnimalType animalType = animal.AnimalId.Type;
            AddToAvailable(animalType);
            
            if (_countDatas.ContainsKey(animalType))
            {
                _countDatas[animalType] = _countDatas[animalType].AddTotal();
            }
            else
            {
                var animalCountData = new AnimalCountData().AddTotal();
                _countDatas.Add(animalType, animalCountData);
            }

            if (_globalSettings.CanLetHungryAnimalsRelease)
            {
                _countDatas[animalType] = _countDatas[animalType].AddReleaseReady();
            }
            else
            {
                _satietyObservers.Add(animal.AnimalId, new AnimalSatietyObserver(animal, this));
            }

            Updated.Invoke(animalType, _countDatas[animalType]);
        }

        public void Unregister(IAnimal animal)
        {
            AnimalId animalId = animal.AnimalId;
            AnimalType animalType = animalId.Type;
            
            if (_countDatas.ContainsKey(animalType) == false)
                throw new ArgumentNullException();
            
            _countDatas[animalType] = _countDatas[animalType].RemoveTotal();

            if (_globalSettings.CanLetHungryAnimalsRelease == false)
            {
                AnimalSatietyObserver animalSatietyObserver = _satietyObservers[animalId];
                animalSatietyObserver.Dispose();
                _satietyObservers.Remove(animalId);
            }
            else
            {
                _countDatas[animalType] = _countDatas[animalType].RemoveReleaseReady();
            }

            Updated.Invoke(animalType, _countDatas[animalType]);
        }

        public AnimalCountData GetAnimalCountData(AnimalType byType)
        {
            if (_countDatas.TryGetValue(byType, out AnimalCountData data))
                return data;

            throw new ArgumentNullException();
        }

        public IReadOnlyDictionary<AnimalType, AnimalCountData> GetAllData() =>
            _countDatas;

        public IReadOnlyCollection<AnimalType> GetAvailableAnimalTypes() =>
            _availableTypes;

        public void Dispose()
        {
            foreach ((_, AnimalSatietyObserver value) in _satietyObservers)
                value.Dispose();
            
            _countDatas.Clear();
            _satietyObservers.Clear();
        }

        private int GetTotalReadyPairsCount()
        {
            int resultCount = 0;
            
            foreach (var countData in _countDatas)
                resultCount += countData.Value.ReleaseReady;

            return resultCount / AnimalPair.PairCount;
        }

        private void AddReleaseReady(AnimalType animalType)
        {
            _countDatas[animalType] = _countDatas[animalType].AddReleaseReady();
            Updated.Invoke(animalType, _countDatas[animalType]);
        }

        private void RemoveReleaseReady(AnimalType animalType)
        {
            _countDatas[animalType] = _countDatas[animalType].RemoveReleaseReady();
            Updated.Invoke(animalType, _countDatas[animalType]);
        }

        private void AddToAvailable(AnimalType animalType)
        {
            if (_availableTypes.Contains(animalType))
                return;
            
            _availableTypes.Add(animalType);
        }

        private class AnimalSatietyObserver : IDisposable
        {
            private readonly IAnimal _animal;
            private readonly AnimalCounter _countData;
            private bool _isFullBar;

            public AnimalSatietyObserver(IAnimal animal, AnimalCounter countData)
            {
                _countData = countData;
                _animal = animal;
                animal.Stats.Satiety.Full += OnFullSatiety;
                animal.Stats.Satiety.Empty += OnEmptySatiety;
            }

            public void Dispose()
            {
                _animal.Stats.Satiety.Full -= OnFullSatiety;
                _animal.Stats.Satiety.Empty -= OnEmptySatiety;

                if (_isFullBar)
                    RemoveFromRelease();
            }

            private void OnEmptySatiety()
            {
                if (_isFullBar)
                {
                    RemoveFromRelease();
                    _isFullBar = false;
                }
            }

            private void OnFullSatiety()
            {
                if (_isFullBar == false)
                {
                    _isFullBar = true;
                    AddToRelease();
                }
            }

            private void AddToRelease() =>
                _countData.AddReleaseReady(_animal.AnimalId.Type);

            private void RemoveFromRelease() =>
                _countData.RemoveReleaseReady(_animal.AnimalId.Type);
        }
    }
}