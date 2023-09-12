using Logic.Animals.AnimalFeeders;
using Logic.Animals.AnimalsBehaviour.AnimalStats;
using Logic.Animals.AnimalsBehaviour.Emotions;
using Logic.Animals.AnimalsStateMachine;
using Logic.Movement;
using Services.StaticData;
using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour
{
    public class Animal : MonoBehaviour, IAnimal
    {
        [SerializeField] private NavMeshMover _mover;
        [SerializeField] private AnimalAnimator _animator;
        [SerializeField] private AnimalStateMachine _stateMachine;
        [SerializeField] private Jumper _jumper;
        [SerializeField] private EmotionProvider _emotionProvider;
        [SerializeField] private StatsProvider _statProvider;
        [SerializeField] private HappinessFactor _happinessFactor;

        private IStaticDataService _staticDataService;
        private PersonalEmotionService _emotionService;
        private AnimalId _animalId;
        private SatietyEmotionObserver _emotionObserver;

        public Transform Transform => transform;
        public AnimalId AnimalId => _animalId;
        public IStatsProvider Stats => _statProvider;
        public HappinessFactor HappinessFactor => _happinessFactor;
        public AnimalStateMachine StateMachine => _stateMachine;
        public NavMeshMover Mover => _mover;
        public AnimalAnimator Animator => _animator;
        public PersonalEmotionService Emotions => _emotionService;

        private void OnDestroy() =>
            _emotionObserver.Dispose();

        public void Construct(AnimalId animalId, BeginStats beginStats, IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            _animalId = animalId;
            _emotionProvider.Construct(_staticDataService.IconByFoodType(animalId.EdibleFood));
            _emotionService = new PersonalEmotionService(_emotionProvider);
            _statProvider.Construct(beginStats);
            
            //TODO: удалить
            _happinessFactor.Construct(_statProvider.Satiety);

            _emotionObserver = new SatietyEmotionObserver(_statProvider.Satiety, _emotionService);
        }

        public void AttachFeeder(AnimalFeeder feeder)
        {
            _stateMachine.Construct(feeder);
            Activate();
        }
        
        public void ForceMove(Transform to) =>
            _stateMachine.ForceMove(to);

        public void Destroy() =>
            Destroy(gameObject);

        private void Activate()
        {
            _jumper.Jump();
        }

        public override string ToString() =>
            $"Animal {_animalId.Type} (id: {_animalId.ID}\nStats:\n" +
            $"Vitality - {_statProvider.Vitality.CurrentNormalized}/1,\n" +
            $"Satiety - {_statProvider.Satiety.CurrentNormalized}/1,\n " +
            $"Peppiness - {_statProvider.Peppiness.CurrentNormalized}/1,\n)";
    }
}