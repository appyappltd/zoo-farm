using System;
using System.Collections.Generic;
using Data.ItemsData;
using Infrastructure.Factory;
using Logic.Animals.AnimalsBehaviour;
using Logic.Bubble;
using Logic.Interactions;
using Logic.Inventory;
using Logic.Movement;
using Services;
using Services.AnimalHouses;
using Tutorial;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Logic.Medicine
{
    [RequireComponent(typeof(Delay))]
    [RequireComponent(typeof(Inventory.Inventory))]
    [RequireComponent(typeof(Storage))]
    [RequireComponent(typeof(ProductReceiver))]
    public class MedicineBed : MonoBehaviour, ITutorialTrigger
    {
        [SerializeField] private List<ItemData> _data = new();
        [SerializeField] private List<Sprite> _sprites = new();

        private Inventory.Inventory inventory;
        private Storage storage;
        private ProductReceiver receiver;
        private int index = 0;
        private bool canTreat = false;

        private IGameFactory gameFactory;
        private IAnimalHouseService houseService;

        public event Action<Animal> AnimalHealed = animal => { };
        public event Action Triggered = () => { };

        private void Awake()
        {
            gameFactory = AllServices.Container.Single<IGameFactory>();
            houseService = AllServices.Container.Single<IAnimalHouseService>();

            inventory = GetComponent<Inventory.Inventory>();
            receiver = GetComponent<ProductReceiver>();
            storage = GetComponent<Storage>();

            GetComponent<Delay>().Complete += player => Treat(player.GetComponent<Inventory.Inventory>());

            inventory.AddItem += item => item.GetComponent<IMover>().GotToPlace += () =>
            {
                canTreat = true;
                receiver.CanTake = false;
                item.GetComponent<BubbleHolder>().GetBubble.ChangeState(_sprites[index]);
            };
        }

        private void SetNewRandomIndex() => index = (index + Random.Range(0, _data.Count)) % _data.Count;

        private void Treat(Inventory.Inventory playerInventory)
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

            var item = playerInventory.Remove();
            var mover = item.GetComponent<IMover>();
            var handAnimal = inventory.Remove();

            mover.Move(storage.GetItemPlace);
            mover.GotToPlace += () =>
            {
                houseService.TakeQueueToHouse(() =>
                {
                    var animalItemData = (AnimalItemData)handAnimal.ItemData;
                    canTreat = false;
                    receiver.CanTake = true;

                    Destroy(handAnimal.gameObject);
                    var animal = gameFactory.CreateAnimal(animalItemData.AnimalType, handAnimal.transform.position)
                        .GetComponent<Animal>();
                    AnimalHealed.Invoke(animal);
                    SetNewRandomIndex();
                    return animal;
                });

                Triggered.Invoke();
                Destroy(handAnimal.GetComponent<BubbleHolder>().GetBubble.gameObject);
                Destroy(item.gameObject);
            };
        }
    }
}
