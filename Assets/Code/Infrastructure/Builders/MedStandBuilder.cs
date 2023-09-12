using Logic.Medical;
using Services.StaticData;
using UnityEngine;

namespace Infrastructure.Builders
{
    public class MedStandBuilder
    {
        private readonly IStaticDataService _staticDataService;

        public MedStandBuilder(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public MedicalToolStand Build(GameObject medicalStandObject, TreatToolId toolId)
        {
            var medicalStand = medicalStandObject.GetComponent<MedicalToolStand>();
            medicalStand.Construct(_staticDataService.MedStandConfigById(toolId));
            return medicalStand;
        }
    }
}