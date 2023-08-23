using Logic.Interactions;
using Ui;
using UnityEngine;

namespace Logic.LevelGoals
{
    public class ReleaseInteractionProvider : MonoBehaviour
    {
        [SerializeField] private HeroInteraction _interaction;
        [SerializeField] private ReleaseIconView _releaseIcon;

        public HeroInteraction Interaction => _interaction;
        public ReleaseIconView ReleaseIcon => _releaseIcon;
    }
}