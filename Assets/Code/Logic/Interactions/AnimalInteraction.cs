using System;
using System.Collections.Generic;
using Logic.Animals.AnimalsBehaviour;
using Observer;

namespace Logic.Interactions
{
    public sealed class AnimalInteraction : ObserverTarget<IAnimal, TriggerObserver> , IInteractionZone
    {
        private readonly HashSet<Action> _interactionSubs = new HashSet<Action>();
        
        public event Action Entered = () => { };
        public event Action Canceled = () => { };
        public event Action Rejected = () => { };
        public event Action<IAnimal> Interacted = _ => { };
        
        event Action IInteractionZone.Interacted
        {
            add => _interactionSubs.Add(value);
            remove => _interactionSubs.Remove(value);
        }

        public float InteractionDelay => default;
        
        protected override void OnTargetEntered(IAnimal animal)
        {
            foreach (Action action in _interactionSubs)
                action.Invoke();
            
            Interacted.Invoke(animal);
        }
    }
}