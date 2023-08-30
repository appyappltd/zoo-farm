using System;
using Logic.Animals.AnimalsBehaviour;
using Observer;
using UnityEngine;

namespace Logic.Interactions
{
    public sealed class AnimalInteraction : ObserverTarget<IAnimal, TriggerObserver>
    {
        public event Action<IAnimal> Interacted = _ => { };

        protected override void OnTargetEntered(IAnimal animal) =>
            Interacted.Invoke(animal);
    }
}