using Code.Logic.NPC.Keepers.KeepersStateMachine;
using Logic;
using Logic.CellBuilding;
using Logic.Player;
using Logic.Storages;
using Observables;
using Services.AnimalHouses;
using UnityEngine;

namespace Code.Logic.NPC.Keepers
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