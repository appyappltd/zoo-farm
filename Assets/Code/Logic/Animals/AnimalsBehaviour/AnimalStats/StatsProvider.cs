using System.Collections.Generic;
using NaughtyAttributes;
using NTC.Global.Cache;
using Progress;
using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour.AnimalStats
{
    public class StatsProvider : MonoCache, IStatsProvider
    {
        private readonly List<IStatOperateRule> _operateRules = new List<IStatOperateRule>();
        
        [SerializeField] private StatIndicator _vitality;
        [SerializeField] private StatIndicator _satiety;
        [SerializeField] private StatIndicator _peppiness;
        [SerializeField] private StatIndicator _age;

#if UNITY_EDITOR
        [SerializeField] [ProgressBar("Vitality", 100f, EColor.Green)]
        private float _vitalityValue;

        [SerializeField] [ProgressBar("Satiety", 100f, EColor.Red)]
        private float _satietyValue;

        [SerializeField] [ProgressBar("Peppiness", 100f, EColor.Violet)]
        private float _peppinessValue;
        
        [SerializeField] [ProgressBar("Age", 100f, EColor.Violet)]
        private float _ageValue;
#endif
        
        public IProgressBarView Vitality => _vitality.ProgressBar;
        public IProgressBarView Satiety => _satiety.ProgressBar;
        public IProgressBarView Peppiness => _peppiness.ProgressBar;
        public IProgressBarView Age => _age.ProgressBar;
        
        protected override void Run()
        {
#if UNITY_EDITOR
            _vitalityValue = _vitality.ProgressBar.Current.Value;
            _satietyValue = _satiety.ProgressBar.Current.Value;
            _peppinessValue = _peppiness.ProgressBar.Current.Value;
            _ageValue = _age.ProgressBar.Current.Value;
#endif

            InitStatOperateRules();
        }

        public void Construct(BeginStats beginStats)
        {
            beginStats.Calculate();
            _vitality.Construct(beginStats.RandomVitality);
            _satiety.Construct(beginStats.RandomSatiety);
            _peppiness.Construct(beginStats.RandomPeppiness);
            _age.Construct(beginStats.RandomAge);
        }
        
        private void InitStatOperateRules()
        {
            _operateRules.Add(new VitalityOperateRule(_vitality, _satiety, _peppiness));
        }
    }
}