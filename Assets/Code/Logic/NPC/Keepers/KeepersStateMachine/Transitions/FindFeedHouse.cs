using System;
using Logic.Animals;
using Services.AnimalHouses;
using StateMachineBase;

namespace Logic.NPC.Keepers.KeepersStateMachine.Transitions
{
    public class FoundNewFeedHouse : Transition
    {
        private readonly IAnimalHouseService _houseService;
        private readonly Action<IAnimalHouse> _applyHouse;

        private bool _isFound;
        private IAnimalHouse _feedHouse;
        
        public FoundNewFeedHouse(IAnimalHouseService houseService, Action<IAnimalHouse> applyHouse)
        {
            _houseService = houseService;
            _applyHouse = applyHouse;
        }

        public override void Enter()
        {
            _isFound = _houseService.TryGetNextFeedHouse(out _feedHouse);
        }

        public override bool CheckCondition()
        {
            if (_isFound)
                _applyHouse.Invoke(_feedHouse);

            return _isFound;
        }
    }
}