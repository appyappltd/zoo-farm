using Logic.Interactions;
using Logic.Interactions.Validators;
using Ui;
using UnityEngine;

namespace Logic.LevelGoals
{
    public class ReleaseInteractionProvider : MonoBehaviour
    {
        [SerializeField] private HeroInteraction _interaction;
        [SerializeField] private ReleaseIconView _releaseIcon;
        [SerializeField] private ReleaseAnimalValidator _releaseAnimalValidator;

        public HeroInteraction Interaction => _interaction;
        public ReleaseIconView ReleaseIcon => _releaseIcon;
        public ReleaseAnimalValidator Validator => _releaseAnimalValidator;
    }
}