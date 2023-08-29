using Logic.Animals;
using UnityEngine;

namespace Logic.Interactions
{
    public class ChoseHouseInteractionProvider : MonoBehaviour, IInteractionZoneProvider
    {
        [SerializeField] private HeroInteraction _heroInteraction;
        [SerializeField] private SpriteRenderer _animalIcon;
        
        private AnimalType _associatedAnimalType;

        public AnimalType AssociatedAnimalType => _associatedAnimalType;
        public HeroInteraction Interaction => _heroInteraction;
        public SpriteRenderer AnimalIcon => _animalIcon;

        public void Construct(AnimalType associatedAnimalType)
        {
            _associatedAnimalType = associatedAnimalType;
        }
    }
}