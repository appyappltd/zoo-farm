using System;
using System.Collections.Generic;
using System.Diagnostics;
using DelayRoutines;
using NaughtyAttributes;
using NTC.Global.Cache;
using Tools;
using UnityEngine;
using Debug = UnityEngine.Debug;

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

        private Action _onEndCallback = () => { };

        private void Awake()
        {
            _towardMover = new TowardMover<float>(0f, 1f, Mathf.Lerp);

            _commandAwaiter = new RoutineSequence()
                .WaitWhile(() => enabled)
                .Then(ExecuteCommand)
                .LoopWhile(() => _commands.Count > 0);
        }

        public void Show(Action onShowCallback = null)
        {
            onShowCallback ??= () => { };
            
            if (enabled)
            {
                _commands.Enqueue(() =>
                {
                    PlayForward();
                    AppendCallback(onShowCallback);
                });

                if (_commandAwaiter.IsActive == false)
                    _commandAwaiter.Play();

                return;
            }

            AppendCallback(onShowCallback);
            PlayForward();
        }

        public void Hide(Action onHideCallback = null)
        {
            onHideCallback ??= () => { };
            
            if (enabled)
            {
                _commands.Enqueue(() =>
                {
                    PlayReverse();
                    AppendCallback(onHideCallback);
                });
                
                if (_commandAwaiter.IsActive == false)
                    _commandAwaiter.Play();
                
                return;
            }
            
            AppendCallback(onHideCallback);
            PlayReverse();
        }

        protected override void Run()
        {
            bool isActive = _towardMover.TryUpdate(Time.smoothDeltaTime * _speed, out float lerpValue);
            _canvasGroup.alpha = _modifyCurve.Evaluate(lerpValue);

            if (isActive != enabled)
            {
                enabled = isActive;
                OnEnd();
            }
        }

        private void OnEnd()
        {
            _onEndCallback.Invoke();
            _onEndCallback = () => { };
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

        private void AppendCallback(Action callback)=>
            _onEndCallback = callback;

        private void ExecuteCommand()
        {
            if (_commands.TryDequeue(out Action action))
                action.Invoke();
        }

        [Button] [Conditional("UNITY_EDITOR")]
        private void TestShow()
        {
            void Log()
            {
                int i = 0;
                i++;

                Debug.Log($"OnShow {i}");
            }
            
            Show(Log);
        }
        
        [Button] [Conditional("UNITY_EDITOR")]
        private void TestHide()
        {
            void Log()
            {
                int i = 0;
                i++;

                Debug.Log($"OnHide {i}");
            }
            
            Hide(Log);
        }
    }
} 