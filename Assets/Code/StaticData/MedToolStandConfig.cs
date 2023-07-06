using Logic.Medicine;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Med Tool Stand Config", fileName = "NewMedToolStandConfig", order = 0)]
    public class MedToolStandConfig : ScriptableObject
    {
        public MedicineToolId Type;
        public Sprite Icon;
    }
}