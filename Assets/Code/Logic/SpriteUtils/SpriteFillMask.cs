using System.Diagnostics;
using NaughtyAttributes;
using NTC.Global.System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Logic.SpriteUtils
{
    public class SpriteFillMask : MonoBehaviour
    {
        [SerializeField] private Transform _mask;
        [SerializeField] private Vector3 _hiddenMaskScale;

        [FormerlySerializedAs("_fullMaskScale")] [SerializeField]
        private Vector3 _shownMaskScale;

#if UNITY_EDITOR
        [SerializeField] [Range(0f, 1f)] private float _testFill;
        private float _prevTestFill;
#endif

        private void Awake() =>
            _mask.localScale = _shownMaskScale;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Mathf.Approximately(_testFill, _prevTestFill))
                return;

            SetFill(_testFill);
            _prevTestFill = _testFill;
        }
#endif

        public void SetFill(float normalizedFill)
        {
            normalizedFill = Mathf.Clamp01(normalizedFill);
            _mask.localScale = Vector3.Lerp(_hiddenMaskScale, _shownMaskScale, normalizedFill);
        }

        public void Activate() =>
            gameObject.Enable();

        public void Deactivate() =>
            gameObject.Disable();

        [Button("CaptureHiddenScale", EButtonEnableMode.Editor)]
        [Conditional("UNITY_EDITOR")]
        private void CaptureHiddenScale() =>
            _hiddenMaskScale = _mask.localScale;

        [Button("CaptureShownScale", EButtonEnableMode.Editor)]
        [Conditional("UNITY_EDITOR")]
        private void CaptureShownScale() =>
            _shownMaskScale = _mask.localScale;
    }
}