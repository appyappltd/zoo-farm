using System;
using System.Collections.Generic;
using Logic.Movement;
using Logic.NPC.Keepers.KeepersStateMachine.States;
using Logic.Storages;
using NaughtyAttributes;
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

        private Func<bool> _foodNeeds;
        private Func<bool> _foodCollected;

        private void Awake()
        {
            SetUp();
        }

        public void BeginFeed(Vector3 housePosition, Vector3 foodPosition, IInventory houseInventory)
        {
            _foodTarget.position = foodPosition;
            _houseTarget.position = housePosition;

            _foodNeeds = () => houseInventory.Weight < houseInventory.MaxWeight;
            _foodCollected = () =>
                _inventoryHolder.Inventory.Weight >= houseInventory.MaxWeight - houseInventory.Weight;
        }

        private void SetUp()
        {
            Transform selfTransform = transform;
            IInventory inventory = _inventoryHolder.Inventory;

            State waiting = new Idle(_animator);
            State wander = new Wander(_animator, _mover, _maxWanderDistance);
            State moveToFood = new MoveTo(_animator, _mover, _foodTarget);
            State moveToHouse = new MoveTo(_animator, _mover, _houseTarget);
            State collect = new ReceiveItems(_animator, inventory);
            State give = new ReceiveItems(_animator, inventory);

            Transition inCollectPlace = new TargetInRange(selfTransform, _foodTarget, _placeOffset);
            Transition inGivePlace = new TargetInRange(selfTransform, _houseTarget, _placeOffset);
            Transition reachTarget = new ReachDestinationTransition(_mover);
            Transition randomDelay = new RandomTimerTransition(_waitingDelayRange.y, _waitingDelayRange.x);
            Transition foodNeeds = new CheckTransition(ref _foodNeeds);
            Transition foodCollected = new CheckTransition(ref _foodCollected);
            Transition emptyInventory = new EmptyInventory(inventory);

            Init(waiting, new Dictionary<State, Dictionary<Transition, State>>
            {
                {
                    waiting, new Dictionary<Transition, State>
                    {
                        {randomDelay, wander},
                        {foodNeeds, moveToFood}
                    }
                },
                {
                    wander, new Dictionary<Transition, State>
                    {
                        {reachTarget, waiting},
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
                        {foodCollected, moveToHouse},
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
    }
}