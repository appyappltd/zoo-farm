using Logic.Animals;
using Logic.Animals.AnimalsBehaviour;
using Logic.Interactions;
using Logic.Player;
using Observables;
using Services;
using Services.AnimalHouses;
using Services.Animals;
using StateMachineBase.States;
using Ui.Services;
using Ui.Windows;
using UnityEngine;

namespace Logic.Breeding
{
    public class BreedingHouse : MonoBehaviour
    {
        private readonly CompositeDisposable _disposable = new CompositeDisposable();
        
        [SerializeField] private Transform _firstPlace;
        [SerializeField] private Transform _secondPlace;
        [SerializeField] private Transform _childPlace;
        
        [SerializeField] private HeroInteraction _heroInteraction;
        private IWindowService _windowService;
        private IAnimalsService _animalService;
        private IAnimalHouseService _houseService;
            
        private IAnimal _first;
        private IAnimal _second;

        private void Awake()
        {
            _windowService = AllServices.Container.Single<IWindowService>();
            _animalService = AllServices.Container.Single<IAnimalsService>();
            _houseService = AllServices.Container.Single<IAnimalHouseService>();

            _heroInteraction.Interacted += OnInteracted;
        }

        private void OnInteracted(Hero _)
        {
            GameObject window = _windowService.Open(WindowId.Breeding);
            window.GetComponent<BreedingWindow>().SetOnChoseCallback(OnAnimalChosen);
        }

        private void OnAnimalChosen(AnimalType type)
        {
            BreedingPair pair = _animalService.SelectPairForBreeding(type);
            MoveToPlace(pair.First, _firstPlace);
            MoveToPlace(pair.Second, _secondPlace);
        }

        private void MoveToPlace(IAnimal animal, Transform place)
        {
            animal.ForceMove(place);
            _houseService.VacateHouse(animal.AnimalId);
            
            _disposable.Add(animal.StateMachine.CurrentStateType.Then(state =>
            {
                if (state == typeof(Idle))
                {
                    animal.StateMachine.Stop();
                }
            }));
        }
    }
}   