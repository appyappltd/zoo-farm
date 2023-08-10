using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Logic.Interactions;
using Logic.Player;
using Services;
using Services.AnimalHouses;
using Services.Animals;
using Ui.Services;
using Ui.Windows;
using UnityEngine;

namespace Logic.Breeding
{
    public class BreedingHouse : MonoBehaviour
    {
        private const int MaxAnimals = 2;

        [SerializeField] private Transform _firstPlace;
        [SerializeField] private Transform _secondPlace;
        [SerializeField] private Transform _childPlace;
        
        [SerializeField] private HumanInteraction _humanInteraction;
        
        private IWindowService _windowService;
        private IAnimalsService _animalService;
        private IAnimalHouseService _houseService;
            
        private IAnimal _first;
        private IAnimal _second;
        
        private int _animalsInHouse;

        private void Awake()
        {
            _windowService = AllServices.Container.Single<IWindowService>();
            _animalService = AllServices.Container.Single<IAnimalsService>();
            _houseService = AllServices.Container.Single<IAnimalHouseService>();

            _humanInteraction.Interacted += OnInteracted;
        }

        private void OnInteracted(Human human)
        {
            if (human is Hero)
            {
                GameObject window = _windowService.Open(WindowId.Breeding);
                window.GetComponent<BreedingWindow>().SetOnChoseCallback(OnAnimalChosen);
            }
        }

        private void OnAnimalChosen(AnimalType type)
        {
            BreedingPair pair = _animalService.SelectPairForBreeding(type);
            MoveToPlace(pair.First, _firstPlace);
            MoveToPlace(pair.Second, _secondPlace);
        }

        private void MoveToPlace(IAnimal animal, Transform place)
        {
            animal.StateMachine.ForceMove(place);
            animal.Mover.DestinationReached += OnDestinationReached;
            animal.Stats.Deactivate();

            _houseService.VacateHouse(animal.AnimalId);

            void OnDestinationReached()
            {
                animal.Mover.DestinationReached -= OnDestinationReached;
                CheckForBeginBreeding();
            }
        }

        private void CheckForBeginBreeding()
        {
            _animalsInHouse++;
            
            if (_animalsInHouse >= MaxAnimals)
                BeginBreeding();
        }

        private void BeginBreeding()
        {
            Debug.Log("Begin breeding");
        }
    }
}   