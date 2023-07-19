using Logic.Medical;
using Services.StaticData;

namespace Infrastructure.Builders
{
    public class MedStandBuilder
    {
        private readonly IStaticDataService _staticDataService;
        private readonly MedicalToolNeedsReporter _reporter;

        public MedStandBuilder(IStaticDataService staticDataService, MedicalToolNeedsReporter reporter)
        {
            _staticDataService = staticDataService;
            _reporter = reporter;
        }

        public void Build(MedicalToolStand medicalStand, MedicalToolId toolId) =>
            medicalStand.Construct(_staticDataService.MedStandConfigById(toolId), _reporter);
    }
}