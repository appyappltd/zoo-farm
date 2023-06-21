using Logic.AnimalsBehaviour.Emotions;
using Logic.AnimalsStateMachine;
using Services.StaticData;
using UnityEngine;

namespace Logic.AnimalsBehaviour
{
    public class Animal : MonoBehaviour
    {
        [SerializeField] private AnimalStateMachine _stateMachine;
        [SerializeField] private Jumper _jumper;
        [SerializeField] private EmotionBubble _emotionBubble;

        private PersonalEmotionService _emotionService;
        private AnimalStateMachineObserver _stateMachineObserver;
        private AnimalId _animalId;
        private IStaticDataService _staticDataService;

        public AnimalId AnimalId => _animalId;

        private void OnDisable()
        {
            _stateMachineObserver.Dispose();
            _emotionService.Unregister(_stateMachineObserver);
        }

        public void Construct(AnimalId animalId, IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _animalId = animalId;
        }

        public void AttachHouse(AnimalHouse house)
        {
            _stateMachine.Construct(house.RestPlace, house.EatPlace);
            _emotionService = new PersonalEmotionService(_staticDataService, _emotionBubble);
            _stateMachineObserver = new AnimalStateMachineObserver(_stateMachine);
            _emotionService.Register(_stateMachineObserver);
            Activate();
        }
        
        private void Activate() =>
            _jumper.Jump();
    }
}