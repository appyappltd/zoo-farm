using UnityEngine;

namespace Logic.SpriteUtils
{
    public class SpriteFillMask : MonoBehaviour
    {
        [SerializeField] private Transform _mask;

        [SerializeField] private Vector3 _hiddenMaskScale;
        [SerializeField] private Vector3 _fullMaskScale;

#if UNITY_EDITOR
        [SerializeField] [Range(0f, 1f)] private float _testFill;
        private float _prevTestFill;
#endif

        private void Awake() =>
            _mask.localScale = _fullMaskScale;

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
            _mask.localScale = Vector3.Lerp(_hiddenMaskScale, _fullMaskScale, normalizedFill);
        }
    }
}