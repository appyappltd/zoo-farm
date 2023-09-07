using System;
using System.Collections.Generic;
using System.Diagnostics;
using DelayRoutines;
using NaughtyAttributes;
using NTC.Global.Cache;
using Tools;
using Unity.VisualScripting;
using UnityEngine;

namespace Ui.Elements
{
    public class FadeOutPanel : MonoCache, IShowable, IHidable
    {
        private readonly Queue<Action> _commands = new Queue<Action>();
        
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _speed;

        [SerializeField] [CurveRange(0, 0, 1, 1, EColor.Green)]
        private AnimationCurve _modifyCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private RoutineSequence _commandAwaiter;
        private TowardMover<float> _towardMover;


        private void Awake()
        {
            _towardMover = new TowardMover<float>(0f, 1f, Mathf.Lerp);

            _commandAwaiter = new RoutineSequence()
                .WaitWhile(() => enabled)
                .Then(ExecuteCommand)
                .LoopWhile(() => _commands.Count > 0);
        }

        public void Show()
        {
            if (enabled)
            {
                _commands.Enqueue(PlayForward);

                if (_commandAwaiter.IsActive == false)
                    _commandAwaiter.Play();

                return;
            }

            PlayForward();
        }

        public void Hide()
        {
            if (enabled)
            {
                _commands.Enqueue(PlayReverse);
                
                if (_commandAwaiter.IsActive == false)
                    _commandAwaiter.Play();
                
                return;
            }
            
            PlayReverse();
        }

        protected override void Run()
        {
            bool isActive = _towardMover.TryUpdate(Time.smoothDeltaTime * _speed, out float lerpValue);
            _canvasGroup.alpha = _modifyCurve.Evaluate(lerpValue);

            if (isActive != enabled)
                enabled = isActive;
        }

        private void PlayForward()
        {
            _towardMover.Forward();
            enabled = true;
        }

        private void PlayReverse()
        {
            _towardMover.Reverse();
            enabled = true;
        }

        private void ExecuteCommand()
        {
            if (_commands.TryDequeue(out Action action))
                action.Invoke();
        }

        [Button] [Conditional("UNITY_EDITOR")]
        private void TestShow()
        {
            Show();
        }
        
        [Button] [Conditional("UNITY_EDITOR")]
        private void TestHide()
        {
            Hide();
        }
    }
} 