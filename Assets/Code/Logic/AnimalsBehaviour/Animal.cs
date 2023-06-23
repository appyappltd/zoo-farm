using Logic.AnimalsBehaviour.AnimalStats;
using Logic.AnimalsBehaviour.Emotions;
using Logic.AnimalsStateMachine;
using Services.StaticData;
using UnityEngine;

namespace Logic.AnimalsBehaviour
{
    public class Animal : MonoBehaviour, IAnimal
    {
        [SerializeField] private AnimalStateMachine _stateMachine;
        [SerializeField] private Jumper _jumper;
        [SerializeField] private EmotionProvider _emotionProvider;
        [SerializeField] private StatsProvider _statProvider;

        private PersonalEmotionService _emotionService;
        private AnimalStateMachineObserver _stateMachineObserver;
        private AnimalId _animalId;
        private IStaticDataService _staticDataService;

        public AnimalId AnimalId => _animalId;
        public StatsProvider Stats => _statProvider;

        private void OnDisable()
        {
            _stateMachineObserver.Dispose();
            _emotionService.Unregister(_stateMachineObserver);
        }

        public void Construct(AnimalId animalId, IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _animalId = animalId;
            _emotionService = new PersonalEmotionService(_emotionProvider);
            _stateMachineObserver = new AnimalStateMachineObserver(_stateMachine);
            _emotionService.Register(_stateMachineObserver);
        }

        public void AttachHouse(AnimalHouse house)
        {
            _stateMachine.Construct(house.RestPlace, house.EatPlace);
            Activate();
        }
        
        private void Activate() =>
            _jumper.Jump();
    }
}