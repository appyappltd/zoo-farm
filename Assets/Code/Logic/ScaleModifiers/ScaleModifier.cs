using Services;
using Services.StaticData;
using StaticData.ScaleModifiers;
using UnityEngine;

namespace Logic.ScaleModifiers
{
    public class ScaleModifier : MonoBehaviour
    {
        [SerializeField] private ScaleModifierId _scaleModifierId;

        private ScaleModifierConfig _modifierConfigById;
        private Vector3 _initialLocalScale;

        private void OnDestroy() =>
            _modifierConfigById.Updated -= ModifyLocalScale;

        private void Awake()
        {
            _initialLocalScale = transform.localScale;
            _modifierConfigById =
                AllServices.Container.Single<IStaticDataService>().ScaleModifierById(_scaleModifierId);

            ModifyLocalScale(_modifierConfigById.ScaleModifier);
            _modifierConfigById.Updated += ModifyLocalScale;
        }

        private void ModifyLocalScale(float modifier) =>
            transform.localScale = _initialLocalScale * modifier;
    }
}