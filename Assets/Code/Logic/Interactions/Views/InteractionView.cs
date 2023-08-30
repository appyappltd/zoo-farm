using System;
using System.Diagnostics;
using AYellowpaper;
using NaughtyAttributes;
using NTC.Global.Cache;
using UnityEngine;

namespace Logic.Interactions
{
    public class InteractionView : MonoCache
    {
        [Header("References")]
        [SerializeField] private InterfaceReference<IInteractionZone, MonoBehaviour> _playerInteraction;
        [SerializeField] private Transform _sine;

        [Space] [Header("Settings")]
        [SerializeField] private bool _isSupportLoopedInteraction;
        [SerializeField] private AxleInfluence _axleInfluence;
        [SerializeField] private float _defaultSize = 1f;
        [SerializeField] private float _maxSize = 1.2f;
        [SerializeField] private float _decreaseTime = 0.15f;
        [SerializeField] private bool _isCustomIncreaseTime;
        
        [ShowIf(nameof(_isCustomIncreaseTime))]
        [SerializeField] private float _increaseTime = 0.15f;
        
        private float _deltaSize;
        private float _targetSize;
        private float _smoothTime;
        private float _sizeDifference;
        
        private Vector3 _axleFilter;
        private Vector3 _initialScale;

        private void Awake()
        {
            _playerInteraction.Value.Entered += OnEnter;
            _playerInteraction.Value.Canceled += OnCancel;
            _playerInteraction.Value.Rejected += OnRejected;
            
            if (_isSupportLoopedInteraction)
                _playerInteraction.Value.Interacted += OnInteracted;

            _increaseTime = _isCustomIncreaseTime
                ? _increaseTime
                : _playerInteraction.Value.InteractionDelay;
            
            _initialScale = _sine.localScale;
            _deltaSize = _defaultSize;
            _sizeDifference = Mathf.Abs(_defaultSize - _maxSize);
            
            SetupFilter();
            SetDefault();
        }

        private void OnDestroy()
        {
            _playerInteraction.Value.Entered -= OnEnter;
            _playerInteraction.Value.Canceled -= OnCancel;
            _playerInteraction.Value.Rejected -= OnRejected;
            
            if (_isSupportLoopedInteraction)
                _playerInteraction.Value.Interacted -= OnInteracted;
            
            SetDefault();
        }

        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            SetupFilter();
        }

        public void SetDefault()
        {
            enabled = false;
            _sine.localScale = ScaleFiltrate(_initialScale, _defaultSize);
            _deltaSize = _defaultSize;
        }

        protected override void Run()
        {
            _deltaSize = Mathf.MoveTowards(_deltaSize, _targetSize, Time.deltaTime / _smoothTime * _sizeDifference);
            _sine.localScale = ScaleFiltrate(_initialScale, _deltaSize);
            
            if (Mathf.Approximately(_deltaSize, _targetSize))
                enabled = false;
        }

        private void OnEnter()
        {
            BeginIncrease();
        }

        private void OnRejected()
        {
            Reset();
        }

        private void OnInteracted()
        {
            SetDefault();
            BeginIncrease();
        }

        private void OnCancel()
        {
            Cancel();
        }

        private void Reset()
        {
            _targetSize = _defaultSize;
            _deltaSize = _defaultSize;
            enabled = false;
        }

        private void BeginIncrease()
        {
            _targetSize = _maxSize;
            _smoothTime = _increaseTime;
            enabled = true;
        }

        private void Cancel()
        {
            _targetSize = _defaultSize;
            _smoothTime = _decreaseTime;
            enabled = true;
        }

        private Vector3 ScaleFiltrate(Vector3 origin, float scale)
        {
            Vector3 inverseFilter = -(_axleFilter - Vector3.one);
            Vector3 filtered = Vector3.Scale(origin, _axleFilter);
            Vector3 scaled = filtered * scale;
            Vector3 invertedFilteredOrigin = Vector3.Scale(origin, inverseFilter);
            return scaled + invertedFilteredOrigin;
        }

        private void SetupFilter()
        {
            _axleFilter = _axleInfluence switch
            {
                AxleInfluence.Both => new Vector3(1, 1, 0),
                AxleInfluence.Vertical => new Vector3(0, 1, 0),
                AxleInfluence.Horizontal => new Vector3(1, 0, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(_axleInfluence))
            };
        }
    }
}
    