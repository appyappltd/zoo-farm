using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Logic.Interactions;
using Logic.Player;
using Logic.Translators;
using NaughtyAttributes;
using UnityEngine;

namespace Logic.Gates
{
    [RequireComponent(typeof(RunTranslator))]
    public class Gate : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<Door> _doors = new List<Door>();
        [SerializeField] private PlayerInteraction _playerInteraction;
        
        [Space] [Header("Settings")] [SerializeField]
        private bool _isAuto;
        
        private ITranslator translator;

        private void Awake()
        {
            translator = GetComponent<RunTranslator>();
            _playerInteraction ??= GetComponent<PlayerInteraction>();
            enabled = _isAuto;
        }

        private void OnEnable()
        {
            _playerInteraction.Interacted += OnInteracted;
            _playerInteraction.Canceled += OnCanceled;
        }
        
        private void OnDisable()
        {
            _playerInteraction.Interacted -= OnInteracted;
            _playerInteraction.Canceled -= OnCanceled;
        }

        private void OnInteracted(Hero _) =>
            Open();

        private void OnCanceled() =>
            Close();

        [Button("Open", enabledMode: EButtonEnableMode.Playmode)]
        public void Open()
        {
            for (var index = 0; index < _doors.Count; index++)
            {
                Door door = _doors[index];
                door.Translatable.Play(door.ClosedLocation, door.OpenedLocation);
                translator.AddTranslatable(door.Translatable);
            }
        }

        [Button("Close", enabledMode: EButtonEnableMode.Playmode)]
        public void Close()
        {
            for (var index = 0; index < _doors.Count; index++)
            {
                Door door = _doors[index];
                door.Translatable.Play(door.OpenedLocation, door.ClosedLocation);
                translator.AddTranslatable(door.Translatable);
            }
        }

        [Button("Collect Doors", enabledMode: EButtonEnableMode.Editor)] [Conditional("UNITY_EDITOR")]
        private void CollectDoorChildes()
        {
            _doors = GetComponentsInChildren<Door>().ToList();
        }   
    }
}