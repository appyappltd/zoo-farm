using System;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.Animals.AnimalFeeders;
using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.Emotions;
using Services.Effects;
using Services.Feeders;
using UnityEngine;

namespace Logic.Breeding
{
    public class BreedingProcess
    {
        private readonly IEffectService _effectService;
        private readonly IGameFactory _gameFactory;
        private readonly IAnimalFeederService _feederService;
        private readonly AnimalPair _pair;
        private readonly Transform _at;
        private readonly Action _onBeginsCallback;

        private int _animalsInPlaceCount;

        public BreedingProcess(IEffectService effectService, IGameFactory gameFactory, IAnimalFeederService feederService, AnimalPair pair, Transform at, Action onBeginsCallback = null)
        {
            _effectService = effectService;
            _gameFactory = gameFactory;
            _feederService = feederService;
            _pair = pair;
            _at = at;
            _onBeginsCallback = onBeginsCallback ?? (() =>{ }) ;
        }
        
        public void Start()
        {
            IAnimal first = _pair.First;
            IAnimal second = _pair.Second;

            first.StateMachine.MoveBreeding(_at, 
                OnAnimalOnPlace,
                () => OnBreedingComplete(first, second));
            second.StateMachine.MoveBreeding(_at, OnAnimalOnPlace, () => { });
            
            first.Emotions.Show(EmotionId.Breeding);
            second.Emotions.Show(EmotionId.Breeding);
        }

        private void OnAnimalOnPlace()
        {
            _animalsInPlaceCount++;

            if (_animalsInPlaceCount >= AnimalPair.PairCount)
                SpawnBreedingEffects();
        }
        
        private void OnBreedingComplete(IAnimal first, IAnimal second)
        {
            Animal newAnimal = _gameFactory.CreateAnimal(first, _at.position, Quaternion.identity);
            AnimalFeeder feeder = _feederService.GetFeeder(newAnimal.AnimalId.EdibleFood);
            newAnimal.AttachFeeder(feeder);
            
            first.Emotions.Suppress(EmotionId.Breeding);
            second.Emotions.Suppress(EmotionId.Breeding);
        }

        private void SpawnBreedingEffects()
        {
            _onBeginsCallback.Invoke();
            _effectService.SpawnEffect(EffectId.Hearts, _at.position, Quaternion.LookRotation(Vector3.up));
        }
    }
}