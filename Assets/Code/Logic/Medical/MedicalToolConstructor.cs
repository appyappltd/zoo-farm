using Data.ItemsData;
using UnityEngine;

namespace Logic.Medical
{
    public class MedicalToolConstructor : MonoBehaviour
    {
        [SerializeField] private HandItem _handItem;
        [SerializeField] private MedicalToolItemData _medicalToolItemData;


        private void Awake() =>
            _handItem.Construct(_medicalToolItemData);
    }
}