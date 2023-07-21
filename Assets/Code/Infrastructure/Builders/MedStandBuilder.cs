using Logic.Medical;
using Services.StaticData;

namespace Infrastructure.Builders
{
    public class MedStandBuilder
    {
        private readonly IStaticDataService _staticDataService;

        public MedStandBuilder(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public void Build(MedicalToolStand medicalStand, MedicalToolId toolId) =>
            medicalStand.Construct(_staticDataService.MedStandConfigById(toolId));
    }
}