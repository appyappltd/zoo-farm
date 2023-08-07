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
        // private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        [SerializeField] private KeeperStateMachine _stateMachine;
        [SerializeField] private InventoryHolder _inventoryHolder;
        [SerializeField] private InventoryAnimatorObserver _handsAnimator;
        [SerializeField] private Storage _storage;
        
        private IAnimalHouseService _animalService;
        private GardenBedGridOperator _gardenGrid;

        private void Awake()
        {
            Init();
            _stateMachine.Construct();
        }

        // private void Awake()
        // {
        //     _animalService = AllServices.Container.Single<IAnimalHouseService>();
        //     _disposable.Add(_stateMachine.CurrentStateType.Then(type =>
        //     {
        //         if (type != typeof(Wander))
        //             return;
        //
        //         if (_animalService.TryGetNextFeedHouse(out AnimalHouse house))
        //             _stateMachine.BeginFeed(house);
        //     }));
        // }
        //
        // private void OnDestroy() =>
        //     _disposable.Dispose();
    }
}