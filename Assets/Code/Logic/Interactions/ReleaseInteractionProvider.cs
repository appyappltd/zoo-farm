using Logic.Interactions.Validators;
using Ui;
using UnityEngine;

namespace Logic.Interactions
{
    public class ReleaseInteractionProvider : MonoBehaviour, IInteractionZoneProvider
    {
        [SerializeField] private HeroInteraction _interaction;
        [SerializeField] private ReleaseIconView _releaseIcon;
        [SerializeField] private ReleaseAnimalValidator _releaseAnimalValidator;

        public HeroInteraction Interaction => _interaction;
        public ReleaseIconView ReleaseIcon => _releaseIcon;
        public ReleaseAnimalValidator Validator => _releaseAnimalValidator;
    }
}