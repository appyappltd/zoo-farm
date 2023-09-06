using System;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Logic.AnimatorStateMachine;
using Logic.Movement;
using StateMachineBase.States;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine.States
{
    public class FollowToBreed : Move
    {
        private readonly StatIndicator _satiety;
        
        private Transform _breedingPlace;
        private Action _onBreedingBegin;

        public FollowToBreed(IPrimeAnimator animator, NavMeshMover mover, StatIndicator satiety) : base(animator, mover)
        {
            _satiety = satiety;
        }

        protected override Vector3 GetMovePosition() =>
            _breedingPlace.position;

        protected override void OnEnter()
        {
            base.OnEnter();
            _satiety.ProgressBar.Reset();
        }

        protected override void OnExit()
        {
            _onBreedingBegin.Invoke();
            base.OnExit();
        }

        public void Init(Transform followAnimal, Action onBreedingBegin)
        {
            _onBreedingBegin = onBreedingBegin;
            _breedingPlace = followAnimal;
        }
    }
}