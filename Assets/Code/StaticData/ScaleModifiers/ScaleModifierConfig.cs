using System;
using UnityEngine;

namespace StaticData.ScaleModifiers
{
    [CreateAssetMenu(menuName = "Static Data/Scale Modifier Config", fileName = "NewScaleModifierConfig", order = 0)]
    public class ScaleModifierConfig : ScriptableObject
    {
        [SerializeField] private ScaleModifierId _modifierId;
        [SerializeField] private float _scaleModifier;

        public event Action<float> Updated = _ => { };

        public ScaleModifierId ModifierId => _modifierId;

        public float ScaleModifier
        {
            get => _scaleModifier;
            set
            {
                _scaleModifier = value;
                Updated.Invoke(value);
            }
        }
    }
}