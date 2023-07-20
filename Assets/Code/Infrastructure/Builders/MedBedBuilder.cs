using Logic.Medical;
using Services.MedicalBeds;

namespace Infrastructure.Builders
{
    public class MedBedBuilder
    {
        private readonly IMedicalBedsReporter _reporter;

        public MedBedBuilder(IMedicalBedsReporter medicalBedsReporter)
        {
            _reporter = medicalBedsReporter;
        }

        public void Build(MedicalBed medicalBed)
        {
            _reporter.Register(medicalBed);
        }
    }
}