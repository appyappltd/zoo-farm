﻿using System.Collections.Generic;
using Logic.Animals;
using Logic.Foods.Vendor;
using Logic.Movement;
using Logic.NPC.Keepers.KeepersStateMachine.States;
using Logic.NPC.Keepers.KeepersStateMachine.Transitions;
using Logic.Storages;
using NaughtyAttributes;
using Services;
using Services.AnimalHouses;
using Services.Food;
using StateMachineBase;
using StateMachineBase.States;
using StateMachineBase.Transitions;
using UnityEngine;

namespace Logic.NPC.Keepers.KeepersStateMachine
{
    public class KeeperStateMachine : StateMachine
    {
        [Header("References")] [SerializeField]
        private NPCAnimator _animator;

        [SerializeField] private NavMeshMover _mover;
        [SerializeField] private InventoryHolder _inventoryHolder;

        [Space] [Header("Settings")] [SerializeField] [Range(.0f, 1f)]
        private float _placeOffset;

        [SerializeField] private float _maxWanderDistance;

        [SerializeField] [MinMaxSlider(1f, 20f)]
        private Vector2 _waitingDelayRange;

        private Transform _foodTarget;
        private Transform _houseTarget;

        private IAnimalHouse _feedHouse;

        private IAnimalHouseService _houseService;
        private IFoodService _foodService;

        public void Construct()
        {
            _foodService = AllServices.Container.Single<IFoodService>();
            _houseService = AllServices.Container.Single<IAnimalHouseService>();

            _foodTarget = new GameObject("KeeperFoodTarget").transform;
            _houseTarget = new GameObject("KeeperHouseTarget").transform;

            SetUp();
        }

        private void SetUp()
        {
            Transform selfTransform = transform;
            IInventory inventory = _inventoryHolder.Inventory;
            inventory.Deactivate();

            State waiting = new Idle(_animator);
            State vendorFinding = new Idle(_animator);
            State houseFinding = new Idle(_animator);
            State wander = new Wander(_animator, _mover, _maxWanderDistance);
            State moveToFood = new MoveTo(_animator, _mover, _foodTarget);
            State moveToHouse = new MoveTo(_animator, _mover, _houseTarget);
            State collect = new ReceiveItems(_animator, inventory);
            State give = new ReceiveItems(_animator, inventory);

            Transition foundNewFeedHouse = new FoundNewFeedHouse(_houseService, AppleFeedHouse);
            Transition inCollectPlace = new TargetInRange(selfTransform, _foodTarget, _placeOffset);
            Transition inGivePlace = new TargetInRange(selfTransform, _houseTarget, _placeOffset);
            Transition reachTarget = new ReachDestinationTransition(_mover);
            Transition randomDelay = new RandomTimerTransition(_waitingDelayRange.y, _waitingDelayRange.x);
            Transition foodNeeds = new CheckTransition(IsFoodNeeds);
            Transition allFoodCollected = new CheckTransition(IsFoodCollected);
            Transition foodNotEnough = new CheckTransition(IsVendorFound);
            Transition emptyInventory = new EmptyInventory(inventory);
            Transition foodItemCollected = new ItemCollected(inventory);

            Init(waiting, new Dictionary<State, Dictionary<Transition, State>>
            {
                {
                    waiting, new Dictionary<Transition, State>
                    {
                        {randomDelay, wander},
                        {foodNeeds, vendorFinding},
                    }
                },
                {
                    wander, new Dictionary<Transition, State>
                    {
                        {reachTarget, waiting},
                        {foundNewFeedHouse, waiting},
                    }
                },
                {
                    moveToFood, new Dictionary<Transition, State>
                    {
                        {inCollectPlace, collect},
                    }
                },
                {
                    collect, new Dictionary<Transition, State>
                    {
                        {foodItemCollected, vendorFinding},
                    }
                },
                {
                    vendorFinding, new Dictionary<Transition, State>
                    {
                        {allFoodCollected, moveToHouse},
                        {foodNotEnough, moveToFood},
                    }
                },
                {
                    moveToHouse, new Dictionary<Transition, State>
                    {
                        {inGivePlace, give},
                    }
                },
                {
                    give, new Dictionary<Transition, State>
                    {
                        {emptyInventory, waiting},
                    }
                },
            });
        }

        private bool IsFoodNeeds()
        {
            bool isFoodNeeds = _feedHouse is {IsTaken: true} &&
                               _feedHouse.Inventory.IsEmpty;
            Debug.Log($"IsFoodNeeds: {isFoodNeeds}");
            return isFoodNeeds;
        }

        private bool IsVendorFound()
        {
            if (_inventoryHolder.Inventory.Weight >= _feedHouse.Inventory.MaxWeight - _feedHouse.Inventory.Weight ||
                _inventoryHolder.Inventory.IsFull)
                return false;

            IFoodVendorView vendor = _foodService.GetReadyVendor(_feedHouse.EdibleFoodType);
            bool isNewVendorFound = vendor != null;

            if (isNewVendorFound && vendor.Position != _foodTarget.position)
                _foodTarget.position = vendor.Position;

            Debug.Log($"IsVendorFound: {isNewVendorFound}");
            return isNewVendorFound;
        }

        private bool IsFoodCollected()
        {
            bool inventoryIsFull = _feedHouse != null
                                   && (_inventoryHolder.Inventory.Weight >=
                                       _feedHouse.Inventory.MaxWeight - _feedHouse.Inventory.Weight
                                       || _inventoryHolder.Inventory.IsFull);
            Debug.Log($"IsFoodCollected: {inventoryIsFull}");
            return inventoryIsFull;
        }

        private void AppleFeedHouse(IAnimalHouse house)
        {
            _feedHouse = house;
            _houseTarget.position = house.FeedingPlace.position;
        }
    }
}