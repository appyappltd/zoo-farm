using Data;
using Data.SaveData;
using Services.PersistentProgress;
using Services.SaveLoad;
using UnityEngine;

namespace Infrastructure.States
{
    public class LoadProgressState : IState
    {
        private readonly GameStateMachine _gameStateMachine;
        private readonly IPersistentProgressService _progressService;
        private readonly ISaveLoadService _saveLoadProgress;

        public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService,
            ISaveLoadService saveLoadProgress)
        {
            _gameStateMachine = gameStateMachine;
            _progressService = progressService;
            _saveLoadProgress = saveLoadProgress;
        }

        public void Enter()
        {
            LoadProgressOrInitNew();
            _gameStateMachine.Enter<LoadLevelState, string>(GetFirstScene());
        }

        public void Exit() { }

        private string GetFirstScene()
        {
            string sceneName;
#if !UNITY_EDITOR
            sceneName = LevelNames.First;
#endif
#if UNITY_EDITOR
            sceneName = _gameStateMachine.EditorInitialScene;
#endif
            return sceneName;
        }

        private void LoadProgressOrInitNew()
        {
            if (_saveLoadProgress.LoadProgress(out GlobalData globalData, out LevelData levelData))
            {
                _progressService.Progress = new PlayerProgress(GetFirstScene())
                {
                    GlobalData = globalData,
                    LevelData = levelData
                };

                return;
            }

            _progressService.Progress = NewProgress();
        }

        private PlayerProgress NewProgress()
        {
            Debug.Log("New Progress");
            var progress = new PlayerProgress(GetFirstScene());
            return progress;
        }
    }
}