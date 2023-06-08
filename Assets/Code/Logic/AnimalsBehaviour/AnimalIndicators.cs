using NaughtyAttributes;
using NTC.Global.Cache;
using Progress;
using Tools;
using UnityEngine;

namespace Logic.AnimalsBehaviour
{
    public class AnimalIndicators : MonoCache
    {
        [SerializeField] [ProgressBar("Vitality", 100f, EColor.Green)] private float _vitalityValue;
        [SerializeField] [ProgressBar("Satiety", 100f, EColor.Red)] private float _satietyValue;
        [SerializeField] [ProgressBar("Tiredness", 100f, EColor.Violet)] private float _peppinessValue;
        
        [SerializeField] [RequireInterface(typeof(IProgressBarView))] private MonoBehaviour _vitality;
        [SerializeField] [RequireInterface(typeof(IProgressBarView))] private MonoBehaviour _satiety;
        [SerializeField] [RequireInterface(typeof(IProgressBarView))] private MonoBehaviour _peppiness;

        private IProgressBarView VitalityBar => (IProgressBarView) _vitality;
        private IProgressBarView SatietyBar => (IProgressBarView) _satiety;
        private IProgressBarView PeppinessBar => (IProgressBarView) _peppiness;
        
        protected override void Run()
        {
            _vitalityValue = VitalityBar.Current;
            _satietyValue = SatietyBar.Current;
            _peppinessValue = PeppinessBar.Current;
        }
    }
}