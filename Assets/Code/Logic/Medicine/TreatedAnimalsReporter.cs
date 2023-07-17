using System.Collections.Generic;

namespace Logic.Medicine
{
    public class TreatedAnimalsReporter
    {
        private readonly List<MedicalBed> _medicalBeds = new List<MedicalBed>();
        
        public void RegisterMedicineBed(MedicalBed bed)
        {
            if (_medicalBeds.Contains(bed))
                return;
            
            _medicalBeds.Add(bed);
            
        }
    }
}