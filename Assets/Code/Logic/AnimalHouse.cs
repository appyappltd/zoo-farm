using System;
using Logic.AnimalsBehaviour;
using UnityEngine;

namespace Logic
{
    public class AnimalHouse : MonoBehaviour
    {
        private const string ThisHouseIsAlreadyTaken = "This house is already taken";

        [SerializeField] private Transform _restPlace;
        [SerializeField] private Transform _eatPlace;
        [SerializeField] private bool _isTaken;

        private AnimalId _animalId;

        public AnimalId AnimalId => _animalId;
        public Transform RestPlace => _restPlace;
        public Transform EatPlace => _eatPlace;
        public bool IsTaken => _isTaken;

        public void AttachAnimal(AnimalId animalId)
        {
            if (_isTaken)
                throw new Exception(ThisHouseIsAlreadyTaken);

            _animalId = animalId;
            _isTaken = true;
        }
    }
}