using NaughtyAttributes;
using Tools;
using UnityEngine;

namespace Logic.Translators
{
    [RequireComponent(typeof(LinearTranslator))]
    [RequireComponent(typeof(CurveTranslator))]
    public class TestTranslator : MonoBehaviour
    {
        [SerializeField] [RequireInterface(typeof(ITranslatable))] private MonoBehaviour _translatable;
        [SerializeField] private float _speed;
        
        [SerializeField] private Transform _from;
        [SerializeField] private Transform _to;

        private ITranslator _linearTranslator;
        private ITranslator _curveTranslator;
        private ITranslator _slerpTranslator;

        private void Awake()
        {
            _linearTranslator = GetComponent<LinearTranslator>();
            _curveTranslator = GetComponent<CurveTranslator>();
            _curveTranslator = GetComponent<SlerpTranslator>();
        }

        // [Button("Translate Linear")]
        // private void TranslateLinear()
        // {
        //     _linearTranslator.Translate(_from.position, _to.position);
        // }
        //
        // [Button("Translate By Curve")]
        // private void TranslateCurve()
        // {
        //     _curveTranslator.Translate(_from.position, _to.position);
        // }
        //
        // [Button("Translate Slerp")]
        // private void TranslateSlerp()
        // {
        //     _curveTranslator.Translate(_from.position, _to.position);
        // }
    }
}