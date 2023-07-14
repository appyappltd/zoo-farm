using Logic.Medicine;

namespace Infrastructure.Builders
{
    public class MedBedBuilder
    {
        private readonly MedicalToolNeedsReporter _reporter;

        public MedBedBuilder(MedicalToolNeedsReporter reporter) =>
            _reporter = reporter;

        public void Build(MedicalBed medicalBed) =>
            _reporter.RegisterMedicineBed(medicalBed);
    }
}