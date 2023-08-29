using System;
using System.Collections.Generic;
using Logic.Animals.AnimalsStateMachine.States;
using Logic.Animals.AnimalsBehaviour.Emotions;
using Observables;

namespace Logic.Animals.AnimalsStateMachine
{
    public class AnimalStateMachineObserver : IDisposable
    {
        private readonly IPersonalEmotionService _emotionService;
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly Dictionary<Type, EmotionId> _emotionsPerState = new Dictionary<Type, EmotionId>
        {
            // [typeof(Rest)] = EmotionId.Sleeping,
            // [typeof(FollowToBreed)] = EmotionId.Breeding,
            // [typeof(BreedingIdle)] = EmotionId.Breeding,
        };

        public AnimalStateMachineObserver(AnimalStateMachine stateMachine, IPersonalEmotionService emotionService)
        {
            _emotionService = emotionService;
            _compositeDisposable.Add(stateMachine.CurrentStateType.Then(OnStateChanged));
            _emotionService.Show(EmotionId.Homeless);
        }

        public void Dispose() =>
            _compositeDisposable.Dispose();

        private void OnStateChanged(Type prev, Type curr)
        {
            if (_emotionsPerState.TryGetValue(prev, out EmotionId emotion))
                _emotionService.Suppress(emotion);

            if (_emotionsPerState.TryGetValue(curr, out emotion))
                _emotionService.Show(emotion);
        }
    }
}