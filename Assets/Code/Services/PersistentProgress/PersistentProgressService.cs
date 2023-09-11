using System;
using System.Collections.Generic;
using System.Reflection;
using Data.SaveData;
using Infrastructure;
using Services.PersistentProgressGeneric;
using Tools.Extension;
using UnityEngine;

namespace Services.PersistentProgress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        private readonly Dictionary<Type, IProgressKey> _progressKeys = new Dictionary<Type, IProgressKey>();

        public PlayerProgress Progress { get; private set; } = new PlayerProgress(LevelNames.Initial);

        public void Init(PlayerProgress progress)
        {
            Progress = progress;
            FindFields(progress);
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
        
        public TProgress GetProgress<TProgress>() where TProgress : IProgressKey =>
            _progressKeys[typeof(TProgress)] as TProgress;
    }
}