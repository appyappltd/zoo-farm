using System;
using Logic.Medical;

namespace Services.MedicalBeds
{
    public interface IMedicalBedsReporter : IService
    {
        void Register(MedicalBed medicalBed);
        bool HasFreeBeds();
        bool IsNeeds(MedicalToolId toolId);
        void Cleanup();
        event Action Updated;
    }
}