using Logic.NPC.Keepers.KeepersStateMachine;
using Services;
using Services.AnimalHouses;
using UnityEngine;

namespace Logic.NPC.Keepers
{
    public class Keeper : MonoBehaviour
    {
        [SerializeField] private KeeperStateMachine _stateMachine;

        private void Awake()
        {
            AllServices.Container.Single<IAnimalHouseService>().TakeQueueToHouse();
        }
    }
}