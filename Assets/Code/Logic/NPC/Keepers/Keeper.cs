using Logic.CellBuilding;
using Logic.NPC.Keepers.KeepersStateMachine;
using Logic.Player;
using Services.AnimalHouses;
using UnityEngine;

namespace Logic.NPC.Keepers
{
    public class Keeper : Human
    {
        [SerializeField] private KeeperStateMachine _stateMachine;

        private IAnimalHouseService _animalService;
        private GardenBedGridOperator _gardenGrid;

        private void Awake()
        {
            Init();
            _stateMachine.Construct();
        }
    }
}