using System;
using System.Diagnostics;
using AYellowpaper;
using NTC.Global.Cache;
using UnityEngine;
using Debug = UnityEngine.Debug;

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
        [SerializeField] private float _defaultSize;
        [SerializeField] private float _maxSize;
        [SerializeField] private float _decreaseTime = 0.25f;

        private float _deltaSize;
        private float _targetSize;
        private float _smoothTime;
        
        private Vector3 _axleFilter;
        private Vector3 _initialSize;

        private void Awake()
        {
            _playerInteraction.Value.Entered += OnEnter;
            _playerInteraction.Value.Canceled += OnCancel;
            _playerInteraction.Value.Rejected += OnRejected;
            
            if (_isSupportLoopedInteraction)
                _playerInteraction.Value.Interacted += OnInteracted;

            _initialSize = _sine.localScale;
            _deltaSize = _defaultSize;
            SetupFilter();
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
            _sine.localScale = ScaleFiltrate(_initialSize, _defaultSize);
            _deltaSize = _defaultSize;
        }

        protected override void Run()
        {
            _deltaSize = Mathf.MoveTowards(_deltaSize, _targetSize, Time.deltaTime / _smoothTime);
            _sine.localScale = ScaleFiltrate(_initialSize, _deltaSize);
            
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
            _smoothTime = _playerInteraction.Value.InteractionDelay;
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
    