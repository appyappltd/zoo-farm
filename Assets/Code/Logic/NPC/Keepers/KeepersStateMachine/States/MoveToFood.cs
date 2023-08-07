using System;
using Logic.Animals;
using Logic.Movement;
using Services.Food;
using StateMachineBase.States;
using UnityEngine;

namespace Logic.NPC.Keepers.KeepersStateMachine.States
{
    internal class MoveToFood : MoveTo
    {
        private readonly NavMeshMover _mover;
        private readonly Transform _target;
        private readonly IFoodService _foodService;
        private readonly Func<AnimalHouse> _getFeedHouse;

        public MoveToFood(NPCAnimator animator, NavMeshMover mover, Transform target, Func<AnimalHouse> getFeedHouse,
            IFoodService foodService) : base(animator, mover, target)
        {
            _mover = mover;
            _target = target;
            _getFeedHouse = getFeedHouse;
            _foodService = foodService;
        }

        protected override void OnEnter()
        {
            IFoodVendor vendor = _foodService.GetReadyVendor(_getFeedHouse.Invoke().EdibleFoodType);
            Debug.Log(vendor);
            _target.position = vendor != null ? vendor.Position : _mover.transform.position;
            base.OnEnter();
        }
    }
}