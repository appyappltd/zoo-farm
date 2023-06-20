using System;
using System.Collections.Generic;
using Infrastructure.Factory;
using Logic;
using Services;
using Services.Camera;
using Services.Input;
using Services.PersistentProgress;
using Services.SaveLoad;

namespace Infrastructure.States
{
    public sealed class GameStateMachine
    {
#if UNITY_EDITOR
        public string EditorInitialScene;
#endif

        private readonly Dictionary<Type, IExitableState> _states;
        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, AllServices services, ICoroutineRunner coroutineRunner,
            LoadingCurtain curtain) =>
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, services, coroutineRunner),
                [typeof(LoadLevelState)] = new LoadLevelState(this,
                    curtain, sceneLoader, services.Single<IGameFactory>(), services.Single<IPlayerInputService>(),
                    services.Single<IPersistentProgressService>(), services.Single<ICameraOperatorService>()),
                [typeof(LoadProgressState)] = new LoadProgressState(this,
                    services.Single<IPersistentProgressService>(),
                    services.Single<ISaveLoadService>()),
                [typeof(GameLoopState)] = new GameLoopState(),
                [typeof(FinishState)] = new FinishState()
            };

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            var state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}