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
            foreach (ISavedProgress progressWriter in _gameFactory.ProgressWriters)
                progressWriter.UpdateProgress(_progressService.Progress);

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

        public PlayerProgress LoadProgress() =>
            PlayerPrefs.GetString(GlobalProgressKey)?
                .ToDeserialized<PlayerProgress>();

        private static string GetCurrentLevelKey() =>
            SceneManager.GetActiveScene().name;
    }
}