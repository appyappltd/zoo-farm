using System;
using System.Collections.Generic;
using System.Linq;
using DelayRoutines;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.Emotions;
using Services.AnimalHouses;
using Services.Animals;
using Services.Effects;
using Tools.Constants;
using UnityEngine;

namespace Services.Breeding
{
    public class AnimalBreedService : IAnimalBreedService
    {
        private readonly IEffectService _effectService;
        private readonly IGameFactory _gameFactory;
        private readonly IAnimalHouseService _houseService;
        private readonly IAnimalsService _animalsService;

        private readonly Dictionary<IAnimal, Action> _disposes = new Dictionary<IAnimal, Action>();
        private readonly List<IAnimal> _breedingReadyAnimals = new List<IAnimal>();

        private IEnumerable<AnimalType> _typesEnumerator = new List<AnimalType>();

        private Vector2 _pairFoundingDelay = new Vector2(3f, 8f);

        private RoutineSequence _pairFounding;
        private bool _isFirstLoad = true;

        public AnimalBreedService(IEffectService effectService, IGameFactory gameFactory, IAnimalHouseService houseService, IAnimalsService animalsService)
        {
            _effectService = effectService;
            _gameFactory = gameFactory;
            _houseService = houseService;
            _animalsService = animalsService;

            _animalsService.Registered += Register;
            _animalsService.Released += Unregister;
        }

        public void Register(IAnimal animal)
        {
            void LocalSatietyOnFull() => OnSatietyFull(animal);
            animal.Stats.Satiety.Full += LocalSatietyOnFull;
            _disposes.Add(animal, () => animal.Stats.Satiety.Full -= LocalSatietyOnFull);
            
            if (_isFirstLoad)
            {
                _isFirstLoad = false;
                _pairFounding = new RoutineSequence();
                _pairFounding.WaitForRandomSeconds(_pairFoundingDelay).Then(FindPairs);
                _pairFounding.Play();
            }
        }

        public void Unregister(IAnimal animal)
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

        private void OnSatietyFull(IAnimal animal) =>
            _breedingReadyAnimals.Add(animal);

        private void FindPairs()
        {
            // Debug.Log("FindPairs");
            
            using IEnumerator<AnimalType> enumerator = _typesEnumerator.GetEnumerator();

            if (enumerator.MoveNext())
                FindSinglePair(enumerator);
            else
                _typesEnumerator = _breedingReadyAnimals.Select(animal => animal.AnimalId.Type).Distinct();
            
            _pairFounding.Play();
        }

        private void FindSinglePair(IEnumerator<AnimalType> enumerator)
        {
            AnimalType animalType = enumerator.Current;

            if (_houseService.HasEmptyHouse(animalType) == false)
            {
                // Debug.Log("No empty house");
                return;
            }

            if (TryFindPair(animalType, out AnimalPair animalPair))
            {
                // Debug.Log($"Pair found {animalPair}");
                BeginBreeding(animalPair.First, animalPair.Second);
                return;
            }
            
            // Debug.Log($"Pair not found");
        }

        private bool TryFindPair(AnimalType breedingAnimalType, out AnimalPair pair)
        {
            IAnimal[] ready = _breedingReadyAnimals.Where(animal => animal.AnimalId.Type == breedingAnimalType && IsBreedingReady(animal)).ToArray();
            pair = new AnimalPair();
            
            if (ready.Count() >= AnimalPair.PairCount)
            {
                pair = new AnimalPair(ready[0], ready[1]);
                return true;
            }

            return false;
        }

        private void BeginBreeding(IAnimal first, IAnimal second)
        {
            _breedingReadyAnimals.Remove(first);
            _breedingReadyAnimals.Remove(second);

            first.StateMachine.MoveBreeding(second, 
                () => OnBreedingBegin(first, second),
                () => OnBreedingComplete(first, second));
            second.StateMachine.MoveBreeding(first, () => { }, () => { });
            
            first.Emotions.Show(EmotionId.Breeding);
            second.Emotions.Show(EmotionId.Breeding);
        }

        private void OnBreedingComplete(IAnimal first, IAnimal second)
        {
            Vector3 centerAnimalsPosition = GetPositionBetweenAnimals(first.Transform, second.Transform);
            Animal newAnimal = _gameFactory.CreateAnimal(first, centerAnimalsPosition, Quaternion.identity);
            _houseService.TakeQueueToHouse(new QueueToHouse(newAnimal, () => { }));
            
            first.Emotions.Suppress(EmotionId.Breeding);
            second.Emotions.Suppress(EmotionId.Breeding);
        }

        private void OnBreedingBegin(IAnimal first, IAnimal second)
        {
            Vector3 centerAnimalsPosition = GetPositionBetweenAnimals(first.Transform, second.Transform);
            _effectService.SpawnEffect(EffectId.Hearts, centerAnimalsPosition, Quaternion.LookRotation(Vector3.up));
        }

        private bool IsBreedingReady(IAnimal animal) =>
            animal.Stats.Satiety.IsEmpty == false;

        private Vector3 GetPositionBetweenAnimals(Transform first, Transform second) =>
            (first.position - second.position) * Arithmetic.ToHalf + second.position;
    }
}