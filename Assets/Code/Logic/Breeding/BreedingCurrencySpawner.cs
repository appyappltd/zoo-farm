using System;
using System.Collections.Generic;
using Data.ItemsData;
using DelayRoutines;
using Infrastructure.Factory;
using Logic.Storages;
using Logic.Storages.Items;
using Logic.Translators;
using NTC.Global.System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Logic.Breeding
{
    public class BreedingCurrencySpawner
    {
        private readonly Stack<HandItem> _breedingCurrencies = new Stack<HandItem>();

        private readonly IHandItemFactory _handItemFactory;
        private readonly Storage _storage;
        private readonly IInventory _inventory;
        private readonly ITranslator _translator;
        private readonly RoutineSequence _spawnRoutine;

        private bool _hasVacateCurrency;
        
        public BreedingCurrencySpawner(IHandItemFactory handItemFactory, Storage storage, IInventory inventory, Vector2 randomSpawnDelay, ITranslator translator)
        {
            _translator = translator;
            _handItemFactory = handItemFactory;
            _storage = storage;
            _inventory = inventory;

            // _inventory.Removed += OnRemoved;

            _spawnRoutine = new RoutineSequence()
                .WaitUntil(() => _breedingCurrencies.Count > 0)
                .WaitForRandomSeconds(randomSpawnDelay)
                .Then(TrySpawn);

            InitBreedingCurrencies();
            _spawnRoutine.Play();
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
        }

        private void TrySpawn()
        {
            Debug.Log("TrySpawn");
            
            if (_breedingCurrencies.TryPop(out HandItem item))
            {
                item.transform.position = _storage.TopPlace.position;

                if (item.TranslatableAgent.Main is CustomScaleTranslatable scaleTranslatable)
                {
                    item.gameObject.Enable();
                    scaleTranslatable.Play(Vector3.zero, Vector3.one);
                    _translator.AddTranslatable(item.TranslatableAgent.Main);
                }

                _inventory.Add(item);

                if (_breedingCurrencies.TryPeek(out _))
                    _spawnRoutine.Play();
            }
        }

        private void OnRemoved(IItem _) =>
            EnableSpawning();

        private void EnableSpawning()
        {
            if (_spawnRoutine.IsActive)
                return;
            
            _spawnRoutine.Play();
        }
    }
}