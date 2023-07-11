using Logic.Medicine;
using Services.StaticData;

namespace Infrastructure.Builders
{
    public class MedStandBuilder
    {
        private readonly IStaticDataService _staticDataService;

        public MedStandBuilder(IStaticDataService staticDataService) =>
            _staticDataService = staticDataService;

        public void Build(MedToolStand medStand, MedicineToolId toolId) =>
            medStand.Construct(_staticDataService.MedStandConfigById(toolId));
    }
}