using Infrastructure.Factory;
using Logic;
using Logic.AnimalsBehaviour;
using Logic.AnimalsStateMachine;
using Services.Input;
using Services.PersistentProgress;
using UnityEngine;

namespace Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IGameFactory _gameFactory;
        private readonly IPlayerInputService _inputService;
        private readonly IPersistentProgressService _progressService;
        private readonly LoadingCurtain _curtain;

        public LoadLevelState(GameStateMachine gameStateMachine, LoadingCurtain curtain, SceneLoader sceneLoader,
            IGameFactory gameFactory, IPlayerInputService inputService, IPersistentProgressService progressService)
        {
            _stateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
            _gameFactory = gameFactory;
            _inputService = inputService;
            _progressService = progressService;
            _curtain = curtain;
        }

        public void Enter(string payload)
        {
            _curtain.Show();
            _gameFactory.Cleanup();
            _gameFactory.WarmUp();
            _sceneLoader.Load(payload, OnLoaded);
        }

        public void Exit() =>
            _curtain.Hide();

        private void OnLoaded()
        {
            InitialGameWorld();
            GameObject hero = InitHero();
            FollowCamera(hero.transform);
            InitialHud();
            _stateMachine.Enter<GameLoopState>();
            InformProgressReaders();
        }

        private void InformProgressReaders()
        {
            foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
                progressReader.LoadProgress(_progressService.Progress);
        }
        
        private void FollowCamera(Transform transform)
        {
            Camera.main.GetComponent<CameraMovement>().Construct(transform);
        }

        private GameObject InitHero()
        {
            GameObject hero = _gameFactory.CreateHero(Vector3.zero);
            hero.GetComponent<PlayerMovement>().Construct(_inputService);
            return hero;
        }

        private void InitialHud()
        {
            var hud = _gameFactory.CreateHud();
            hud.GetComponent<Canvas>().worldCamera = Camera.main;

            var inputReader = hud.GetComponentInChildren<IInputReader>();
            _inputService.RegisterInputReader(inputReader);
        }

        private void InitialGameWorld()
        {
            AnimalHouse house = _gameFactory.CreateAnimalHouse(new Vector3(4, 0, 4)).GetComponent<AnimalHouse>();
            GameObject animal = _gameFactory.CreateAnimal(AnimalType.CatB, Vector3.zero);
            AnimalId animalId = new AnimalId(AnimalType.CatB, animal.GetHashCode());
            animal.GetComponent<Animal>().Construct(animalId ,house);
            house.AttachAnimal(animalId);
        }
    }
}