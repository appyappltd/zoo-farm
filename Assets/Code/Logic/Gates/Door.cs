using System.Diagnostics;
using Logic.Translators;
using NaughtyAttributes;
using UnityEngine;

namespace Logic.Gates
{
    [RequireComponent(typeof(LocationTranslatable))]
    public class Door : MonoBehaviour
    {
        [SerializeField] private Location _closedLocation;
        
        [SerializeField] private Location _openedLocation;

        private ITranslatableParametric<Location> _translatable;
        
        public Location OpenedLocation => _openedLocation;
        public Location ClosedLocation => _closedLocation;
        public ITranslatableParametric<Location> Translatable => _translatable;
        
        private void Awake() =>
            _translatable = GetComponent<ITranslatableParametric<Location>>();

        [Button("Save Opened Location")] [Conditional("UNITY_EDITOR")]
        private void SaveOpenedLocation() =>
            _openedLocation = new Location(transform.localPosition, transform.localRotation);
        
        [Button("Save Closed Location")] [Conditional("UNITY_EDITOR")]
        private void SaveClosedLocation() =>
            _closedLocation = new Location(transform.localPosition, transform.localRotation);

        [Button("Close")] [Conditional("UNITY_EDITOR")]
        private void Close()
        {
            transform.localPosition = _closedLocation.Position;
            transform.localRotation = _closedLocation.Rotation;
        }
        
        [Button("Open")] [Conditional("UNITY_EDITOR")]
        private void Open()
        {
            transform.localPosition = _openedLocation.Position;
            transform.localRotation = _openedLocation.Rotation;
        }
    }
}