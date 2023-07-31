using Services;
using Services.StaticData;
using UnityEngine;

namespace StaticData.ScaleModifiers
{
    public class RuntimeScaleModifier : MonoBehaviour
    {
        [SerializeField] private ScaleModifierId _scaleModifierId;
        [SerializeField] private float _scaleModifier;

        private float _prevScaleModifier;
        private ScaleModifierId _prevScaleModifierId;

        private ScaleModifierConfig _scaleModifierConfig;

        private void Awake()
        {
            _prevScaleModifierId = _scaleModifierId;
            _scaleModifierConfig =
                AllServices.Container.Single<IStaticDataService>().ScaleModifierById(_scaleModifierId);
        }

        private void OnValidate()
        {
            if (_scaleModifierConfig is null)
                return;

            if (Mathf.Approximately(_scaleModifier, _prevScaleModifier) == false)
            {
                _scaleModifierConfig.ScaleModifier = _scaleModifier;
                _prevScaleModifier = _scaleModifier;
            }

            if (_scaleModifierId != _prevScaleModifierId)
            {
                _prevScaleModifierId = _scaleModifierId;
                _scaleModifierConfig = LoadConfig();
            }
        }

        private ScaleModifierConfig LoadConfig() =>
            AllServices.Container.Single<IStaticDataService>().ScaleModifierById(_scaleModifierId);
    }
}