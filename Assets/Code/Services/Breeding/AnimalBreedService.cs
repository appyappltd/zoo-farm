using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Logic.Breeding;
using Services.Animals;
using Services.Camera;
using Services.Effects;
using Services.Feeders;
using UnityEngine;
using UnityEngine.Pool;

namespace Services.Breeding
{
    public class AnimalBreedService : IAnimalBreedService
    {
        private readonly IEffectService _effectService;
        private readonly IGameFactory _gameFactory;
        private readonly IAnimalFeederService _feederService;
        private readonly ICameraOperatorService _cameraService;

        private readonly Dictionary<IAnimal, Action> _disposes = new Dictionary<IAnimal, Action>();
        private readonly List<IAnimal> _breedingReadyAnimals = new List<IAnimal>();

        public AnimalBreedService(IEffectService effectService, IGameFactory gameFactory, IAnimalFeederService feederService, IAnimalsService animalsService, ICameraOperatorService cameraService)
        {
            _cameraService = cameraService;
            _effectService = effectService;
            _gameFactory = gameFactory;
            _feederService = feederService;

            animalsService.Registered += Register;
            animalsService.Released += Unregister;
        }

        private void Register(IAnimal animal)
        {
            void OnSatietyFull() => _breedingReadyAnimals.Add(animal);
            void OnSatietyEmpty() => _breedingReadyAnimals.Remove(animal);
            
            animal.Stats.Satiety.Full += OnSatietyFull;
            animal.Stats.Satiety.Empty += OnSatietyEmpty;
            
            _disposes.Add(animal, () =>
            {
                animal.Stats.Satiety.Full -= OnSatietyFull;
                animal.Stats.Satiety.Empty -= OnSatietyEmpty;
            });
        }

        private void Unregister(IAnimal animal)
        {
            if (_disposes.TryGetValue(animal, out Action dispose))
            {
                dispose.Invoke();

                if (_breedingReadyAnimals.Contains(animal))
                    _breedingReadyAnimals.Remove(animal);

                return;
            }

            throw new ArgumentNullException(nameof(dispose));
        }

        public ICollection<AnimalType> GetAvailablePairTypes()
        {
            using (ListPool<AnimalType>.Get(out List<AnimalType> result))
            {
                IEnumerable<IGrouping<AnimalType, IAnimal>> groups = _breedingReadyAnimals.GroupBy(animal => animal.AnimalId.Type);
                
                foreach (IGrouping<AnimalType, IAnimal> group in groups)
                {
                    Debug.Log($"Group: {group.Key}, count: {group.Count()}");
                    
                    if (group.Count() >= AnimalPair.PairCount)
                    {
                        Debug.Log("Add result");
                        result.Add(group.Key);
                    }
                    
                }

                Debug.Log(result.Count);
                return result.ToArray();
            }
        }

        public bool TryBreeding(AnimalType breedingAnimalType, out AnimalPair pair)
        {
            IAnimal[] ready = _breedingReadyAnimals.Where(animal => animal.AnimalId.Type == breedingAnimalType).ToArray();

            if (ready.Count() >= AnimalPair.PairCount)
            {
                pair =  new AnimalPair(ready[0], ready[1]);
                return true;
            }

            pair = new AnimalPair();
            return false;
        }

        public void BeginBreeding(AnimalPair pair, BreedingPositions at, Action onBeginsCallback = null, Action onCompleteCallback = null)
        {
            _breedingReadyAnimals.Remove(pair.First);
            _breedingReadyAnimals.Remove(pair.Second);

            BreedingProcess breedingProcess = new BreedingProcess(_effectService, _gameFactory, _cameraService, _feederService, pair,
                at, onBeginsCallback, onCompleteCallback);
            breedingProcess.Start();
        }
    }
}