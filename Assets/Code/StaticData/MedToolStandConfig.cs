using Logic.Medical;
using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(menuName = "Static Data/Med Tool Stand Config", fileName = "NewMedToolStandConfig", order = 0)]
    public class MedToolStandConfig : ScriptableObject
    {
        public MedicalToolId Type;
        public Sprite Icon;
    }
}