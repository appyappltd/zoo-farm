using System;
using Logic.AnimalsStateMachine;
using UnityEngine;

namespace Logic.AnimalsBehaviour
{
    public class Animal : MonoBehaviour
    {
        [SerializeField] private AnimalStateMachine _stateMachine;
        [SerializeField] private Jumper _jumper;

        private AnimalId _animalId;

        private void Start()
        {
            _jumper.Jump();
        }

        public void Construct(AnimalId animalId, AnimalHouse house)
        {
            _animalId = animalId;
            _stateMachine.Construct(house.RestPlace, house.EatPlace);
        }
    }
}