using System;
using System.Collections.Generic;
using Logic.AnimalsBehaviour.Emotions;
using Logic.AnimalsStateMachine.States;
using Observables;

namespace Logic.AnimalsStateMachine
{
    public class AnimalStateMachineObserver : IEmotive, IDisposable
    {
        private readonly CompositeDisposable _compositeDisposable = new CompositeDisposable();
        private readonly Dictionary<Type, Emotions> _emotionsPerState = new Dictionary<Type, Emotions>
        {
            [typeof(Eat)] = Emotions.Eating,
            [typeof(Rest)] = Emotions.Sleeping,
            [typeof(Idle)] = Emotions.Healthy,
        };

        public event Action<Emotions> ShowEmotion = e => { };

        public event Action<Emotions> SuppressEmotion = e => { };

        public AnimalStateMachineObserver(AnimalStateMachine stateMachine) =>
            _compositeDisposable.Add(stateMachine.CurrentStateType.Then(OnStateChanged));

        public void Dispose() =>
            _compositeDisposable.Dispose();

        private void OnStateChanged(Type prev, Type curr)
        {
            if (_emotionsPerState.TryGetValue(prev, out Emotions emotion))
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