using Logic.Interactions;
using Logic.Player;
using Logic.Storages;
using UnityEngine;

namespace Logic.Upgrades
{
    public abstract class HeroUpgrade : MonoBehaviour
    {
        [SerializeField] private HeroInteraction _interactionZone;
        [SerializeField] private int _improveValue;

        private void Awake()
        {
            _interactionZone.Interacted += OnInteracted;
            OnAwake();
        }

        protected abstract IImprovable GetImprovableFrom(Hero hero);
        protected virtual void OnAwake() { }
        protected virtual void OnImproved() { }

        private void OnInteracted(Hero hero)
        {
            IImprovable improvable = GetImprovableFrom(hero);
            improvable.Improve(_improveValue);
            OnImproved();
        }
    }
}