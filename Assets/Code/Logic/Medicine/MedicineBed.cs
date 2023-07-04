using System.Collections.Generic;
using Data.ItemsData;
using Infrastructure.Factory;
using Logic.Animals.AnimalsBehaviour;
using Logic.Bubble;
using Logic.Interactions;
using Logic.Movement;
using Logic.Storages;
using Services;
using Services.AnimalHouses;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Logic.Medicine
{
    [RequireComponent(typeof(Inventory))]
    [RequireComponent(typeof(Storage))]
    [RequireComponent(typeof(ProductReceiver))]
    public class MedicineBed : MonoBehaviour
    {
        [SerializeField] private List<ItemData> _data = new();
        [SerializeField] private List<Sprite> _sprites = new();
        [SerializeField] private PlayerInteraction _playerInteraction;

        private Inventory inventory;
        private Storage storage;
        private ProductReceiver receiver;
        private int index = 0;
        private bool canTreat = false;

        private IGameFactory gameFactory;
        private IAnimalHouseService houseService;

        private void Awake()
        {
            gameFactory = AllServices.Container.Single<IGameFactory>();
            houseService = AllServices.Container.Single<IAnimalHouseService>();

            inventory = GetComponent<Inventory>();
            receiver = GetComponent<ProductReceiver>();
            storage = GetComponent<Storage>();

            _playerInteraction.Interacted += player => Treat(player.Inventory);

            inventory.Added += item => item.Mover.Ended += () =>
            {
                canTreat = true;
                item.GetComponent<BubbleHolder>().GetBubble.ChangeState(_sprites[index]);
            };
        }

        private void SetNewRandomIndex() => index = (index + Random.Range(0, _data.Count)) % _data.Count;

        private void Treat(Inventory playerInventory)
        {
            if (!canTreat)
                return;
            if (!inventory.CanGiveItem())
                return;
            if (!playerInventory.CanGiveItem(_data[index].Creature))
                return;
            if (playerInventory.GetData.Hand.GetComponent<Medicine>().Type !=
                _data[index].Hand.GetComponent<Medicine>().Type)
                return;

            var item = playerInventory.Get();
            var mover = item.GetComponent<IItemMover>();
            var handAnimal = inventory.Get();

            mover.Move(storage.GetItemPlace);
            mover.Ended += () =>
            {
                houseService.TakeQueueToHouse(() =>
                {
                    var animalItemData = (AnimalItemData)handAnimal.ItemData;
                    canTreat = false;
                    receiver.CanTake = true;

                    Destroy(handAnimal.gameObject);
                    var animal = gameFactory.CreateAnimal(animalItemData.AnimalType, handAnimal.transform.position)
                        .GetComponent<Animal>();

                    SetNewRandomIndex();
                    return animal;
                });
                
                Destroy(handAnimal.GetComponent<BubbleHolder>().GetBubble.gameObject);
                Destroy(item.gameObject);
            };
        }
    }
}
