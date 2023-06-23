using Progress;
using UnityEngine;

namespace Logic.AnimalsBehaviour.AnimalStats
{
    public class StatsProvider : MonoBehaviour
    {
        [SerializeField] private ProgressBarIndicator _vitality;
        [SerializeField] private ProgressBarIndicator _satiety;
        [SerializeField] private ProgressBarIndicator _peppiness;

        public IProgressBarView Vitality => _vitality.ProgressBar;
        public IProgressBarView Satiety => _satiety.ProgressBar;
        public IProgressBarView Peppiness => _peppiness.ProgressBar;
    }
}