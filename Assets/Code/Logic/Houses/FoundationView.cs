using AYellowpaper;
using Logic.CellBuilding.Foundations;
using Logic.CellBuilding.Foundations.Views;
using Logic.Interactions;
using Logic.Player;
using Logic.TransformGrid;
using UnityEngine;

namespace Logic.Houses
{
    public abstract class FoundationView : MonoBehaviour
    {
        [SerializeField] protected InterfaceReference<ITransformGrid, MonoBehaviour> TransformGrid;
        [SerializeField] private HeroInteraction _heroInteraction;
        
        private IFoundation _foundation;
        private readonly HouseFoundationView _houseFoundationView;

        private void Awake()
        {
            OnAwake();
            _foundation = CreateFoundation();
            Subscribe();
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        protected abstract IFoundation CreateFoundation();
        protected virtual void OnAwake() { }

        private void Subscribe()
        {
            _heroInteraction.Interacted += OnInteracted;
            _heroInteraction.Canceled += OnCanceled;
        }

        private void Unsubscribe()
        {
            _heroInteraction.Interacted -= OnInteracted;
            _heroInteraction.Canceled -= OnCanceled;
            
            _foundation.Dispose();
        }

        private void OnInteracted(Hero _) =>
            _foundation.ShowBuildChoice();

        private void OnCanceled() =>
            _foundation.HideBuildChoice();
    }
}