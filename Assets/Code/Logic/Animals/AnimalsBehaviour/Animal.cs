using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Logic.Animals.AnimalsBehaviour.Emotions;
using Logic.Animals.AnimalsStateMachine;
using Services.StaticData;
using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour
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
        public IStatsProvider Stats => _statProvider;

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

        public void Destroy()
        {
            Destroy(gameObject);
        }

        private void Activate() =>
            _jumper.Jump();

        public override string ToString()
        {
            return
                $"Animal {_animalId.Type} (id: {_animalId.ID}\nStats:\n" +
                $"  Vitality - {_statProvider.Vitality.CurrentNormalized}/1,\n" +
                $"  Satiety - {_statProvider.Satiety.CurrentNormalized}/1,\n " +
                $" Peppiness - {_statProvider.Peppiness.CurrentNormalized}/1,\n)";
        }
    }
}