namespace Logic.Animals.AnimalsBehaviour.AnimalStats
{
    public class VitalityOperateRule : IStatOperateRule
    {
        private readonly StatIndicator _vitalityStat;
        private readonly StatIndicator _satietyStat;
        private readonly StatIndicator _peppinessStat;

        public VitalityOperateRule(StatIndicator vitalityStat, StatIndicator satietyStat, StatIndicator peppinessStat)
        {
            _vitalityStat = vitalityStat;
            _satietyStat = satietyStat;
            _peppinessStat = peppinessStat;
            
            satietyStat.ProgressBar.Empty += OnBarEmpty;
            peppinessStat.ProgressBar.Empty += OnBarEmpty;
            
            satietyStat.ProgressBar.Full += OnBarFull;
            peppinessStat.ProgressBar.Full += OnBarFull;
        }

        private void OnBarEmpty()
        {
            _vitalityStat.Disable();
        }

        private void OnBarFull()
        {
            bool isRegenerate = !_peppinessStat.ProgressBar.IsEmpty && !_satietyStat.ProgressBar.IsEmpty;

            if (isRegenerate)
            {
                _vitalityStat.Enable();
            }
        }

        ~VitalityOperateRule()
        {
            _satietyStat.ProgressBar.Empty -= OnBarEmpty;
            _peppinessStat.ProgressBar.Empty -= OnBarEmpty;
            
            _satietyStat.ProgressBar.Full -= OnBarFull;
            _peppinessStat.ProgressBar.Full -= OnBarFull;
        }
    }
}