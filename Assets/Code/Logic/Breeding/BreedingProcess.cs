using System;
using DelayRoutines;
using Infrastructure.Factory;
using Logic.Animals;
using Logic.Animals.AnimalFeeders;
using Logic.Animals.AnimalsBehaviour;
using Logic.Animals.AnimalsBehaviour.Emotions;
using NTC.Global.System;
using Services.Camera;
using Services.Effects;
using Services.Feeders;
using Tools;
using UnityEngine;

namespace Logic.Breeding
{
    public class BreedingProcess
    {
        private const float BeforeBreedingDelaySeconds = 0.25f;
        private const float WarpOffset = 0f;

        private readonly IEffectService _effectService;
        private readonly IGameFactory _gameFactory;
        private readonly IAnimalFeederService _feederService;
        private readonly ICameraOperatorService _cameraService;
        
        private readonly RoutineSequence _beforeBreedingDelay;
        private readonly AnimalPair _pair;
        private readonly BreedingPositions _at;
        private readonly Action _onBeginsCallback;
        private readonly Action _onCompleteCallback;
        private readonly IShowHide _scaleChanger;

        private int _animalsInPlaceCount;
        private bool _isRunning;
        private Animal _child;

        public BreedingProcess(IEffectService effectService, IGameFactory gameFactory, ICameraOperatorService cameraService,
            IAnimalFeederService feederService, AnimalPair pair, BreedingPositions at, Action onBeginsCallback = null, Action onCompleteCallback = null)
        {
            _effectService = effectService;
            _gameFactory = gameFactory;
            _cameraService = cameraService;
            _feederService = feederService;
            _pair = pair;
            _at = at;
            _scaleChanger = _at.Child.GetComponent<IShowHide>();
            _scaleChanger.Hide();
            _onBeginsCallback = onBeginsCallback ?? (() => { });
            _onCompleteCallback = onCompleteCallback ?? (() => { });

            _beforeBreedingDelay = new RoutineSequence()
                .WaitForSeconds(BeforeBreedingDelaySeconds)
                .Then(StartBreedingState);
        }
        
        public void Start()
        {
            IAnimal first = _pair.First;
            IAnimal second = _pair.Second;

            TryWarpAnimalToCameraEdge(first);
            TryWarpAnimalToCameraEdge(second);
            
            first.StateMachine.InitBreeding(_at.First, OnAnimalOnPlace);
            second.StateMachine.InitBreeding(_at.Second, OnAnimalOnPlace);
            
            first.Emotions.Show(EmotionId.Breeding);
            second.Emotions.Show(EmotionId.Breeding);
        }

        private void OnAnimalOnPlace()
        {
            if (_isRunning)
                return;

            _animalsInPlaceCount++;
            
            if (_animalsInPlaceCount >= AnimalPair.PairCount)
            {
                _isRunning = true;
                _beforeBreedingDelay.Play();
            }
        }
        
        private void OnBreedingComplete()
        {
            IAnimal first = _pair.First;
            IAnimal second = _pair.Second;

            _child.transform.localScale = Vector3.one;
            _scaleChanger.Show(() =>
            {
                AnimalFeeder feeder = _feederService.GetFeeder(_child.AnimalId.EdibleFood);
                _child.AttachFeeder(feeder);
                _child.transform.Unparent();
            });

            first.Emotions.Suppress(EmotionId.Breeding);
            second.Emotions.Suppress(EmotionId.Breeding);

            _onCompleteCallback.Invoke();
            _isRunning = false;
        }

        private void StartBreedingState()
        {
            var first = _pair.First;
            var second = _pair.Second;

            first.StateMachine.BeginBreeding(OnBreedingComplete);
            second.StateMachine.BeginBreeding(() => { });
            
            _onBeginsCallback.Invoke();
            _effectService.SpawnEffect(EffectId.Hearts, _at.Center.position, Quaternion.LookRotation(Vector3.up));
            
            _child = _gameFactory.CreateAnimal(first, _at.Child.position, _at.Child.rotation);
            _child.transform.SetParent(_at.Child, true);
            _scaleChanger.Hide();
        }

        private void TryWarpAnimalToCameraEdge(IAnimal first)
        {
            if (first.IsVisible)
                return;

            Vector3 animalPosition = first.Transform.position;
            Vector3 toCameraDirection = _at.Center.position - animalPosition;
            Vector3 warpPoint = _cameraService.GetClosestRayPoint(new Ray(animalPosition, toCameraDirection), WarpOffset);
            first.Mover.Warp(warpPoint);
            Debug.Log($"Animal {first} warp from {animalPosition}, to {warpPoint}");
        }
    }
}