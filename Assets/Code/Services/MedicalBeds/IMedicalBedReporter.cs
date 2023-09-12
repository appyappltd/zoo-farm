using System;
using Logic.Medical;

namespace Services.MedicalBeds
{
    public interface IMedicalBedsReporter : IService
    {
        void Register(MedicalBed medicalBed);
        bool HasFreeBeds();
        bool IsNeeds(TreatToolId toolId);
        void Cleanup();
        event Action Updated;
    }
}