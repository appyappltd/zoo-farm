using Logic.AnimalsStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour
{
    public class Animal : MonoBehaviour
    {
        [SerializeField] private AnimalStateMachine _stateMachine;
        [SerializeField] private Jumper _jumper;

        private AnimalId _animalId;

        public AnimalId AnimalId => _animalId;

        public void Construct(AnimalId animalId) =>
            _animalId = animalId;

        public void AttachHouse(AnimalHouse house)
        {
            _stateMachine.Construct(house.RestPlace, house.EatPlace);
            Activate();
        }

        private void Activate() =>
            _jumper.Jump();
    }
}