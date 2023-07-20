using System;
using Services;
using Services.MedicalBeds;
using UnityEngine;

namespace Logic.Interactions.Validators
{
    public class AnimalBedValidator : MonoBehaviour, IInteractionValidator
    {
        private IMedicalBedsReporter _medicalBedsReporter;

        private void Awake()
        {
            _medicalBedsReporter = AllServices.Container.Single<IMedicalBedsReporter>();
            Debug.Log(_medicalBedsReporter);
        }

        private void Update()
        {
            Debug.Log(_medicalBedsReporter);
        }

        public bool IsValid()
        {
            Debug.Log(_medicalBedsReporter);
            return _medicalBedsReporter.HasFreeBeds();
        }
    }
}