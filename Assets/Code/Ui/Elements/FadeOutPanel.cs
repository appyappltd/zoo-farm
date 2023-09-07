using System.Diagnostics;
using NaughtyAttributes;
using NTC.Global.Cache;
using Tools;
using UnityEngine;

namespace Ui.Elements
{
    public class FadeOutPanel : MonoCache, IShowable, IHidable
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _speed;

        [SerializeField] [CurveRange(0, 0, 1, 1, EColor.Green)]
        private AnimationCurve _modifyCurve = AnimationCurve.Linear(0, 0, 1, 1);

        private TowardMover<float> _towardMover;

        private void Awake() =>
            _towardMover = new TowardMover<float>(0f, 1f, Mathf.Lerp);

        public void Show()
        {
            _towardMover.Forward();
            enabled = true;
        }

        public void Hide()
        {
            _towardMover.Reverse();
            enabled = true;
        }
        
        protected override void Run()
        {
            if (_towardMover.TryUpdate(Time.smoothDeltaTime * _speed, out float lerpValue))
                _canvasGroup.alpha = _modifyCurve.Evaluate(lerpValue);
            else
                enabled = false;
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