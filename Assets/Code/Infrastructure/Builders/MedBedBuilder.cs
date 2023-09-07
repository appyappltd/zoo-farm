using Logic.Medical;
using Services.MedicalBeds;
using UnityEngine;

namespace Infrastructure.Builders
{
    public class MedBedBuilder
    {
        private readonly IMedicalBedsReporter _reporter;

        public MedBedBuilder(IMedicalBedsReporter medicalBedsReporter)
        {
            _reporter = medicalBedsReporter;
        }

        public MedicalBed Build(GameObject medicalBedObject)
        {
            MedicalBed medicalBed = medicalBedObject.GetComponent<MedicalBed>();
            _reporter.Register(medicalBed);
            return medicalBed;
        }
    }
}