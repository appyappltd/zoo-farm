using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Logic.AnimatorStateMachine;
using Logic.Movement;
using StateMachineBase.States;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine.States
{
    public class MoveToBreedPlace : Move
    {
        private readonly StatIndicator _satiety;
        
        private Transform _followAnimal;
        
        public MoveToBreedPlace(IPrimeAnimator animator, NavMeshMover mover, StatIndicator satiety) : base(animator, mover)
        {
            _satiety = satiety;
        }

        protected override Vector3 GetMovePosition() =>
            _followAnimal.position;

        protected override void OnEnter()
        {
            base.OnEnter();
            _satiety.ProgressBar.Reset();
        }

        public void Init(Transform followAnimal) =>
            _followAnimal = followAnimal;
    }
}