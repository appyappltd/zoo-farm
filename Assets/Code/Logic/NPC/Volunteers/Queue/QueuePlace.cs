using System;
using AYellowpaper;
using Logic.Interactions;
using Logic.Interactions.Views;
using Logic.Player;
using Logic.Storages;
using Logic.Translators;
using NTC.Global.System;
using UnityEngine;

namespace Logic.NPC.Volunteers.Queue
{
    public class QueuePlace : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<ITranslator, MonoBehaviour> _translator;
        [SerializeField] private InterfaceReference<ITranslatableParametric<Vector3>, MonoBehaviour> _scaleTranslatable;
        [SerializeField] private HumanInteraction _playerInteraction;
        [SerializeField] private GameObject _interactionView;
        [SerializeField] private InteractionView _interactionSine;
        [SerializeField] private InteractionViewRejector _interactionRejector;
        [SerializeField] private ProductReceiver _productReceiver;

        private bool _isShown;
        
        public event Action<QueuePlace> Vacated = _ => { };
        public event Action<QueuePlace> Hidden = _ => { };
        
        private void Awake()
        {
            _playerInteraction.transform.localScale = Vector3.zero;
            _playerInteraction.Interacted += OnInteracted;
        }

        private void OnHidden(ITranslatable translatable)
        {
            translatable.End -= OnHidden;
            Hidden.Invoke(this);
        }

        private void OnDestroy()
        {
            _playerInteraction.Interacted -= OnInteracted;
        }

        public void Construct(IInventory inventory)
        {
            _productReceiver.Construct(inventory);
        }

        public void Show()
        {
            if (_isShown)
                return;

            _isShown = true;
            _scaleTranslatable.Value.Play(Vector3.zero, Vector3.one * 1.4f);
            _translator.Value.Add(_scaleTranslatable.Value);
            //_playerInteraction.Enable();
            //_playerInteraction.gameObject.Enable();
            // _interactionView.Enable();
            _interactionRejector.Enable();
            _interactionRejector.SetDefault();
            _interactionSine.SetDefault();
        }

        public void Hide()
        {
            if (_isShown == false)
                return;

            _isShown = false;
            _scaleTranslatable.Value.Play(Vector3.one * 1.4f, Vector3.zero);
            _translator.Value.Add(_scaleTranslatable.Value);
            _interactionRejector.Disable();
            // _playerInteraction.Disable();
            // _playerInteraction.gameObject.Disable();
            // _interactionView.Disable();
        }

        private void OnInteracted(Human _)
        {
            Vacated.Invoke(this);
            _scaleTranslatable.Value.End += OnHidden;
        }
    }
}