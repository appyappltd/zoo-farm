using System;
using System.Collections.Generic;
using Data.SaveData;
using Infrastructure.Factory;
using Services.PersistentProgress;
using Services.PersistentProgressGeneric;
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

        private readonly Dictionary<Type, ProgressObserver<IProgressKey>> _progressObservers = new();

        public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory)
        {
            _progressService = progressService;
            _gameFactory = gameFactory;
        }

        public void Register<TProgress>(ISavedProgressReaderGeneric<TProgress> reader) where TProgress : IProgressKey
        {
            if (_progressObservers.TryGetValue(typeof(TProgress), out ProgressObserver<IProgressKey> progressObserver))
            {
                ProgressObserver<TProgress> observer = progressObserver as ProgressObserver<TProgress>;
                observer!.Add(reader);
            }
            else
            {
                _progressObservers.Add(typeof(TProgress),
                    new ProgressObserver<TProgress>(reader) as ProgressObserver<IProgressKey>);
            }
        }

        public void SaveProgress()
        {
            foreach (ISavedProgress progressWriter in _gameFactory.ProgressWriters)
                progressWriter.UpdateProgress(_progressService.Progress);

            foreach (var observer in _progressObservers)
            {

            }

            string globalDataLastLevel = GetCurrentLevelKey();
            _progressService.Progress.GlobalData.LastLevel = globalDataLastLevel;
            
            Debug.Log("save");

            SaveGlobal();
            SaveLevel(globalDataLastLevel);
        }

        public bool LoadProgress(out GlobalData globalData, out LevelData levelData)
        {
            globalData = LoadGlobal();
            levelData = default;

            if (globalData is null)
                return false;

            levelData = LoadLevel(globalData.LastLevel);
            return true;
        }

        private void SaveGlobal() =>
            PlayerPrefs.SetString(GlobalProgressKey, _progressService.Progress.GlobalData.ToJson());

        private void SaveLevel(string levelKey) =>
            PlayerPrefs.SetString(levelKey, _progressService.Progress.LevelData.ToJson());

        private GlobalData LoadGlobal() => 
            PlayerPrefs.GetString(GlobalProgressKey)?
                .ToDeserialized<GlobalData>();

        private LevelData LoadLevel(string levelKey) => 
            PlayerPrefs.GetString(levelKey)?
                .ToDeserialized<LevelData>();

        private static string GetCurrentLevelKey() =>
            SceneManager.GetActiveScene().name;
    }
}