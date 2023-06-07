using Data;
using Infrastructure.Factory;
using Services.PersistentProgress;
using Tools.Extension;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        private const string GlobalProgressKey = "GlobalProgress";

        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _gameFactory;

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
        }

        public void SaveProgress()
        {
            foreach (ISavedProgressGeneric progressWriter in _gameFactory.ProgressWritersGeneric)
                progressWriter.UpdateProgress(_progressService);

            Debug.Log("save");

            SaveGlobal();
            SaveLevel(GetCurrentLevelKey());
        }

        private void SaveGlobal()
        {
            PlayerPrefs.SetString(GlobalProgressKey, _progressService.Progress.ToJson());
        }

        private void SaveLevel(string levelKey)
        {
            PlayerPrefs.SetString(levelKey, _progressService.Progress.LevelData.ToJson());
        }

        public bool LoadProgress(out GlobalData globalData, out LevelData levelData)
        {
            globalData = LoadGlobal();
            levelData = LoadLevel();

            return globalData is not null;
        }

        private GlobalData LoadGlobal()
        {
            return PlayerPrefs.GetString(GlobalProgressKey)?
                .ToDeserialized<GlobalData>();
        }

        private LevelData LoadLevel()
        {
            return PlayerPrefs.GetString(GetCurrentLevelKey())?
                .ToDeserialized<LevelData>();
        }

        private static string GetCurrentLevelKey()
        {
            // return SceneManager.GetActiveScene().name;
            return "TestScene";
        }
    }
}