using Logic.Medical;
using Services.StaticData;
using UnityEngine;

namespace Code.Infrastructure.Builders
{
    public class MedStandBuilder
    {
        private readonly IStaticDataService _staticDataService;

        public MedStandBuilder(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public MedicalToolStand Build(GameObject medicalStandObject, MedicalToolId toolId)
        {
            var medicalStand = medicalStandObject.GetComponent<MedicalToolStand>();
            medicalStand.Construct(_staticDataService.MedStandConfigById(toolId));
            return medicalStand;
        }
    }
}