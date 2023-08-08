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
        private readonly Transform _target;
        private readonly IFoodService _foodService;
        private readonly Func<AnimalHouse> _getFeedHouse;

        private IFoodVendorView _vendor;

        public MoveToFood(NPCAnimator animator, NavMeshMover mover, Transform target, Func<AnimalHouse> getFeedHouse,
            IFoodService foodService) : base(animator, mover, target)
        {
            _target = target;
            _getFeedHouse = getFeedHouse;
            _foodService = foodService;
        }

        protected override void OnEnter()
        {
            IFoodVendorView vendor = _foodService.GetReadyVendor(_getFeedHouse.Invoke().EdibleFoodType);

            // if (vendor == _vendor)
            //     return;

            if (vendor != null)
            {
                _target.position = vendor.Position;
                base.OnEnter();
            }
        }
    }
}