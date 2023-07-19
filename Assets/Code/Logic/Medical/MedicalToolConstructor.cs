using Data.ItemsData;
using UnityEngine;

namespace Logic.Medical
{
    public class MedicineToolConstructor : MonoBehaviour
    {
        [SerializeField] private HandItem _handItem;
        [SerializeField] private MedToolItemData _medToolItemData;


        private void Awake() =>
            _handItem.Construct(_medToolItemData);
    }
}