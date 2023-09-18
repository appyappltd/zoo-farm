using System;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Logic.AnimatorStateMachine;
using Logic.Movement;
using StateMachineBase.States;
using UnityEngine;

namespace Logic.Animals.AnimalsStateMachine.States
{
    public class FollowToBreed : MoveToAndRotate
    {
        private readonly StatIndicator _satiety;
        
        private Action _onBreedingBegin;

        public FollowToBreed(IPrimeAnimator animator, NavMeshMover mover, Transform target, Aligner aligner, StatIndicator satiety) : base(animator, mover, target, aligner)
        {
            _satiety = satiety;
        }

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

        public void Init(Transform breedingPlace, Action onBreedingBegin)
        {
            _onBreedingBegin = onBreedingBegin;
            Target = breedingPlace;
        }
    }
}