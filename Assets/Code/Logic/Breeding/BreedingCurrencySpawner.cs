using System;
using System.Collections.Generic;
using Data.AnimalCounter;
using DelayRoutines;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.Storages;
using Logic.Storages.Items;
using Logic.Translators;
using NTC.Global.System;
using Services.Animals;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Logic.Breeding
{
    public class BreedingCurrencySpawner : IDisposable 
    {
        private readonly Stack<HandItem> _breedingCurrencies = new Stack<HandItem>();

        private readonly IHandItemFactory _handItemFactory;
        private readonly IAnimalCounter _animalCounter;
        private readonly Storage _storage;
        private readonly IInventory _inventory;
        private readonly ITranslator _translator;
        private readonly RoutineSequence _spawnRoutine;

        private bool _hasVacateCurrency;
        private int _maxCurrenciesCount;
        private int _readyPairsCount;

        public BreedingCurrencySpawner(IHandItemFactory handItemFactory, IAnimalCounter animalCounter, Storage storage, IInventory inventory, Vector2 randomSpawnDelay, ITranslator translator)
        {
            _handItemFactory = handItemFactory;
            _animalCounter = animalCounter;
            _storage = storage;
            _inventory = inventory;
            _translator = translator;
            
            _spawnRoutine = new RoutineSequence(RoutineUpdateMod.FixedRun)
                .WaitUntil(HasNewPair)
                .Then(TrySpawn)
                .WaitForRandomSeconds(randomSpawnDelay)
                .LoopWhile(HasFreeCurrency);

            Subscribe();
            InitBreedingCurrencies();
            EnableSpawning();
        }

        public void Dispose()
        {
            Unsubscribe();
            _spawnRoutine.Kill();
        }

        public void ReturnItem(IItem item)
        {
            if (item is HandItem handItem)
            {
                if (_breedingCurrencies.Contains(handItem))
                    throw new InvalidOperationException();

                handItem.gameObject.Disable();
                _breedingCurrencies.Push(handItem);
                EnableSpawning();
                return;
            }

            throw new NullReferenceException(nameof(handItem));
        }

        private void InitBreedingCurrencies()
        {
            int itemsWeight = 0;
            
            while (itemsWeight < _inventory.MaxWeight)
            {
                HandItem item =
                    _handItemFactory.CreateBreedingCurrency(Vector3.zero, Quaternion.identity);
                
                item.gameObject.Disable();
                itemsWeight += item.Weight;

                if (itemsWeight <= _inventory.MaxWeight)
                    _breedingCurrencies.Push(item);
                else
                    Object.Destroy(item);
            }

            
            _maxCurrenciesCount = _breedingCurrencies.Count;
            _readyPairsCount = _animalCounter.TotalBreedingReadyPairsCount;
        }

        private void Subscribe() =>
            _animalCounter.Updated += OnAnimalCounterUpdated;

        private void Unsubscribe() =>
            _animalCounter.Updated += OnAnimalCounterUpdated;

        private void OnAnimalCounterUpdated(AnimalType _, AnimalCountData __) =>
            _readyPairsCount = _animalCounter.TotalBreedingReadyPairsCount;

        private bool HasFreeCurrency() =>
            _breedingCurrencies.TryPeek(out _);

        private bool HasNewPair()
        {
            // Debug.Log($"breedingCurrencies.Count - {_breedingCurrencies.Count}, _readyPairsCount - {_readyPairsCount}, _maxCurrenciesCount - {_maxCurrenciesCount}, spawnedCount - {_maxCurrenciesCount - _breedingCurrencies.Count}");
            return _breedingCurrencies.Count > 0 && _readyPairsCount > _maxCurrenciesCount - _breedingCurrencies.Count;
        }

        private void TrySpawn()
        {
            if (_breedingCurrencies.TryPop(out HandItem item))
            {
                Spawn(item);
            }
        }

        private void Spawn(HandItem item)
        {
            item.transform.position = _storage.TopPlace.position;

            if (item is ITranslatableAnimated animated)
            {
                item.gameObject.Enable();
                ITranslatableParametric<Vector3> animatedScaleTranslatable = animated.ScaleTranslatable;
                animatedScaleTranslatable.Play(Vector3.zero, Vector3.one);
                _translator.Add(animatedScaleTranslatable);
            }

            _inventory.Add(item);
        }

        private void EnableSpawning()
        {
            if (_spawnRoutine.IsActive)
                return;
            
            _spawnRoutine.Play();
        }
    }
}