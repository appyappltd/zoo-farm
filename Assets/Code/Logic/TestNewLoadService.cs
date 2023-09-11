using System.Reflection;
using Data.SaveData;
using Tools.Extension;
using UnityEngine;

namespace Logic
{
    public class TestNewLoadService : MonoBehaviour
    {
        private void Awake()
        {
            var progress = new PlayerProgress("lol");

            foreach (FieldInfo field in progress.GetFields())
            {
                Debug.Log("TEST PROGRESS");
                Debug.Log($"{field.Name} - {field.GetValue(progress)}");
            }
        }
    }
}