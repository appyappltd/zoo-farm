using NaughtyAttributes;
using NTC.Global.Cache;
using UnityEngine;

namespace Logic.AnimalsBehaviour.AnimalStats
{
    public class AnimalIndicators : MonoCache
    {
        [SerializeField] private ProgressBarIndicator _vitality;
        [SerializeField] private ProgressBarIndicator _satiety;
        [SerializeField] private ProgressBarIndicator _peppiness;

#if UNITY_EDITOR
        [SerializeField] [ProgressBar("Vitality", 100f, EColor.Green)]
        private float _vitalityValue;

        [SerializeField] [ProgressBar("Satiety", 100f, EColor.Red)]
        private float _satietyValue;

        [SerializeField] [ProgressBar("Peppiness", 100f, EColor.Violet)]
        private float _peppinessValue;
#endif

        public ProgressBarIndicator Vitality => _vitality;
        public ProgressBarIndicator Satiety => _satiety;
        public ProgressBarIndicator Peppiness => _peppiness;

#if UNITY_EDITOR
        protected override void Run()
        {
            _vitalityValue = _vitality.ProgressBar.Current.Value;
            _satietyValue = _satiety.ProgressBar.Current.Value;
            _peppinessValue = _peppiness.ProgressBar.Current.Value;
        }
#endif
    }
}