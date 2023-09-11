using System.Reflection;
using Data.SaveData;
using Services.PersistentProgressGeneric;
using Tools.Extension;
using UnityEngine;

namespace Logic
{
    public class TestNewLoadService : MonoBehaviour
    {
        private PlayerProgress _progress;

        private void Awake()
        {
            _progress = new PlayerProgress("lol");

            Debug.Log("TEST PROGRESS");
            FindFields(_progress);
            Debug.Log("TEST END");
        }

        private void FindFields(object obj)
        {
            foreach (FieldInfo field in obj.GetFields())
            {
                Debug.Log($"{field.Name} - {field.GetValue(obj)}");

                if (typeof(IProgressKey).IsAssignableFrom(field.FieldType))
                {
                    FindFields(field.GetValue(obj));
                }
            }
        }
    }
} 