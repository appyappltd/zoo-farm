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

        public FoundNewFeedHouse(IAnimalHouseService houseService, Action<IAnimalHouse> applyHouse)
        {
            _houseService = houseService;
            _applyHouse = applyHouse;
        }

        public override bool CheckCondition()
        {
            bool found = _houseService.TryGetNextFeedHouse(out IAnimalHouse feedHouse);

            if (found)
                _applyHouse.Invoke(feedHouse);

            return found;
        }
    }
}