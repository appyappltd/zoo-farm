using Services;
using Services.MedicalBeds;
using UnityEngine;

namespace Logic
{
    public class HomeIcon : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _icon;
        
        private IMedicalBedsReporter _medicalBedsReporter;

        private void Awake()
        {
            _medicalBedsReporter = AllServices.Container.Single<IMedicalBedsReporter>();
            _icon.enabled = !_medicalBedsReporter.HasFreeBeds();
            _medicalBedsReporter.Updated += OnUpdated;
        }

        private void OnDestroy()
        {
            _medicalBedsReporter.Updated -= OnUpdated;
        }

        private void OnUpdated()
        {
            Debug.Log(!_medicalBedsReporter.HasFreeBeds());
            _icon.enabled = !_medicalBedsReporter.HasFreeBeds();
        }
    }
}