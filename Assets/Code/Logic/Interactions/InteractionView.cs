using AYellowpaper;
using NTC.Global.Cache;
using UnityEngine;

namespace Logic.Interactions
{
    public class InteractionView : MonoCache
    {
        [SerializeField] private InterfaceReference<IInteractionZone, MonoBehaviour> _playerInteraction;
        [SerializeField] private Transform _sine;

        [SerializeField] private float _defaultSize;
        [SerializeField] private float _maxSize;
        [SerializeField] private float _decreaseTime = 0.25f;

        private float _deltaSize;
        private float _targetSize;
        private float _smoothTime;

        private void Awake()
        {
            _playerInteraction.Value.Entered += OnEnter;
            _playerInteraction.Value.Canceled += OnCancel;

            _deltaSize = _defaultSize;
        }

        private void OnDestroy()
        {
            _playerInteraction.Value.Entered -= OnEnter;
            _playerInteraction.Value.Canceled -= OnCancel;
            
            SetDefault();
        }

        public void SetDefault()
        {
            enabled = false;
            _sine.localScale = Vector3.one * _defaultSize;
            _deltaSize = _defaultSize;
        }

        private void OnCancel()
        {
            Cancel();
        }

        private void OnEnter()
        {
            BeginIncrease();
        }

        protected override void Run()
        {
            _deltaSize = Mathf.MoveTowards(_deltaSize, _targetSize, Time.deltaTime / _smoothTime);
            _sine.localScale = Vector3.one * _deltaSize;
            
            if (Mathf.Approximately(_deltaSize, _targetSize))
            {
                enabled = false;
            }
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
    }
}
    