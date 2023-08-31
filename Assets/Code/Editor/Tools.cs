using Data;
using Data.SaveData;
using Services;
using Services.SaveLoad;
using UnityEditor;
using UnityEngine;

namespace Code.Editor
{
    public static class Tools
    {
        [MenuItem("Tools/Save")]
        private static void Save()
        {
            AllServices.Container.Single<ISaveLoadService>().SaveProgress();
        }

        [MenuItem("Tools/Load")]
        private static void Load()
        {
            AllServices.Container.Single<ISaveLoadService>().LoadProgress(out GlobalData globalData, out LevelData levelData);
        }
        
        [MenuItem("Tools/ClearPrefs")]
        private static void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}