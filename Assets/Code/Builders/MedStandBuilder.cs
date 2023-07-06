using Logic.Medicine;
using Logic.Storages;
using Services.StaticData;

namespace Builders
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