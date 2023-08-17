using AYellowpaper;
using NTC.Global.Cache;
using UnityEngine;

namespace Logic.Interactions
{
    public class InteractionViewRejector : MonoCache
    {
        [SerializeField] private InterfaceReference<IInteractionZone, MonoBehaviour> _playerInteraction;
        [SerializeField] private SpriteRenderer _sineSprite;
        [SerializeField] private Transform _sineTransform;
        [SerializeField] private AnimationCurve _blinkCurve;
        [SerializeField] private float _blinkSize;
        
        [SerializeField] private Color _rejectedColor;
        [SerializeField] private float _blinkDuration;

        private Color _defaultColor;
        private Vector3 _defaultScale;
        private float _elapsedTime;
        private Color _nextColor;
        private Color _previousColor;
        
        private void Awake()
        {
            _defaultScale = _sineTransform.localScale;
            _defaultColor = _sineSprite.color;
            _playerInteraction.Value.Rejected += OnRejected;
        }

        private void OnDestroy() =>
            _playerInteraction.Value.Rejected -= OnRejected;

        public void SetDefault()
        {
            enabled = false;
            _elapsedTime = float.MaxValue;
        }

        protected override void Run()
        {
            _elapsedTime += Time.deltaTime;

            if (_elapsedTime >= _blinkDuration)
                enabled = false;

            float blinkTime = _elapsedTime / _blinkDuration;
            float delta = _blinkCurve.Evaluate(blinkTime);
            
            _sineSprite.color = Color.Lerp(_defaultColor, _rejectedColor, delta);
            _sineTransform.localScale = Vector3.Lerp(_defaultScale, _defaultScale * _blinkSize, delta);
        }

        private void OnRejected()
        {
            _elapsedTime = 0;
            enabled = true;
        }
    }
}