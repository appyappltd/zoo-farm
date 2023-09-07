using AYellowpaper;
using NaughtyAttributes;
using Progress;
using Tutorial.StaticTriggers;
using UnityEngine;

namespace Tutorial
{
    public class TutorialProgressBarTriggerContainer : MonoBehaviour 
    {
        [SerializeField] private InterfaceReference<IProgressBarProvider, MonoBehaviour> _barViewProvider;
        
        [SerializeField] private bool _isTriggerOnFull;
        [SerializeField] [ShowIf("_isTriggerOnFull")]
        private TutorialTriggerScriptableObject _staticTriggerFull;
        
        [SerializeField] private bool _isTriggerOnEmpty;
        [SerializeField] [ShowIf("_isTriggerOnEmpty")]
        private TutorialTriggerScriptableObject _staticTriggerEmpty;
        
        private void OnEnable()
        {
            if (_isTriggerOnEmpty)
                _barViewProvider.Value.ProgressBarView.Empty += OnEmpty;

            if (_isTriggerOnFull)
                _barViewProvider.Value.ProgressBarView.Full += OnFull;
        }

        private void OnDisable()
        {
            if (_isTriggerOnEmpty)
                _barViewProvider.Value.ProgressBarView.Empty -= OnEmpty;

            if (_isTriggerOnFull)
                _barViewProvider.Value.ProgressBarView.Full -= OnFull;
        }

        private void OnEmpty() =>
            _staticTriggerEmpty.Trigger(gameObject);

        private void OnFull() =>
            _staticTriggerFull.Trigger(gameObject);
    }
}