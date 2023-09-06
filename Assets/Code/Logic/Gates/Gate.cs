using System;
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
        [SerializeField] private HumanInteraction _playerInteraction;

        [Space] [Header("Settings")]
        [SerializeField] private bool _isAuto;
        [SerializeField] private bool _isOpenOnEnter;

        private ITranslator _translator;

        private void Awake()
        {
            _translator = GetComponent<RunTranslator>();
            enabled = _isAuto;
        }

        private void OnEnable()
        {
            if (_isOpenOnEnter)
                _playerInteraction.Entered += OnEntered;
            else
                _playerInteraction.Interacted += OnInteracted;

            _playerInteraction.Canceled += OnCanceled;
        }

        private void OnDisable()
        {
            if (_isOpenOnEnter)
                _playerInteraction.Entered -= OnEntered;
            else
                _playerInteraction.Interacted -= OnInteracted;
            
            _playerInteraction.Canceled -= OnCanceled;
        }

        [Button("Open", enabledMode: EButtonEnableMode.Playmode)]
        public void Open()
        {
            for (var index = 0; index < _doors.Count; index++)
            {
                Door door = _doors[index];
                door.Translatable.Play(door.CurrentLocation, door.OpenedLocation);
                _translator.Add(door.Translatable);
            }
        }

        [Button("Close", enabledMode: EButtonEnableMode.Playmode)]
        public void Close()
        {
            for (var index = 0; index < _doors.Count; index++)
            {
                Door door = _doors[index];
                door.Translatable.Play(door.CurrentLocation, door.ClosedLocation);
                _translator.Add(door.Translatable);
            }
        }

        [Button("Collect Doors", enabledMode: EButtonEnableMode.Editor)]
        [Conditional("UNITY_EDITOR")]
        private void CollectDoorChildes()
        {
            _doors = GetComponentsInChildren<Door>().ToList();
        }

        private void OnEntered()
        {
            Open();
        }

        private void OnInteracted(Human _) =>
            Open();

        private void OnCanceled()
        {
           Close();
        }
    }
}