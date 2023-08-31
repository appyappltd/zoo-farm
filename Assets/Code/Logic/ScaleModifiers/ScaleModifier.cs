using System.Linq;
using Services;
using Services.StaticData;
using StaticData.ScaleModifiers;
using UnityEngine;

namespace Logic.ScaleModifiers
{
    [DisallowMultipleComponent]
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

#if UNITY_EDITOR
            var copy = GetComponents<ScaleModifier>();

            if (copy.Count() > 1)
            {
                Debug.LogWarning("Delete copy of ScaleModifier");
            }
#endif
        }

        private void ModifyLocalScale(float modifier) =>
            transform.localScale = _initialLocalScale * modifier;
    }
}