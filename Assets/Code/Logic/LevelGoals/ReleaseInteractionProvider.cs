using Logic.Interactions;
using UnityEngine;

namespace Logic.LevelGoals
{
    public class ReleaseInteractionProvider : MonoBehaviour
    {
        [SerializeField] private HeroInteraction _interaction;
        [SerializeField] private SpriteRenderer _animalIcon;

        public HeroInteraction Interaction => _interaction;
        public SpriteRenderer AnimalIcon => _animalIcon;
    }
}