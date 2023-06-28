using System;
using System.Collections.Generic;
using Logic.Animals.AnimalsBehaviour.Emotions;
using Logic.Animals.AnimalsStateMachine.States;
using Observables;
using StateMachineBase.States;

namespace Logic.Animals.AnimalsStateMachine
{
    public class AnimalStateMachineObserver : IEmotive, IDisposable
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly Dictionary<Type, EmotionId> _emotionsPerState = new Dictionary<Type, EmotionId>
        {
            [typeof(Eat)] = EmotionId.Eating,
            [typeof(Rest)] = EmotionId.Sleeping,
            [typeof(Idle)] = EmotionId.Healthy,
        };

        public event Action<EmotionId> ShowEmotion = e => { };

        public event Action<EmotionId> SuppressEmotion = e => { };

        public AnimalStateMachineObserver(AnimalStateMachine stateMachine) =>
            _compositeDisposable.Add(stateMachine.CurrentStateType.Then(OnStateChanged));

        public void Dispose() =>
            _compositeDisposable.Dispose();

        private void OnStateChanged(Type prev, Type curr)
        {
            if (_emotionsPerState.TryGetValue(prev, out EmotionId emotion))
            {
                SuppressEmotion.Invoke(emotion);
            }
            
            if (_emotionsPerState.TryGetValue(curr, out emotion))
            {
                ShowEmotion.Invoke(emotion);
            }
        }
    }
}