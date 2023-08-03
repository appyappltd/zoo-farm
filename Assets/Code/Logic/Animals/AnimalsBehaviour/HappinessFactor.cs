using Logic.Animals.AnimalsBehaviour.AnimalStats;
using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour
{
    public class HappinessFactor : MonoBehaviour
    {
        [SerializeField] private StatIndicator _satiety;
        [SerializeField] private bool _isEnableDecrementation;

        private int _factor;

        public int Factor => _factor;

        private void OnEnable()
        {
            _satiety.ProgressBar.Empty += DecrementFactor;
            _satiety.ProgressBar.Full += IncrementFactor;
        }

        private void OnDisable()
        {
            _satiety.ProgressBar.Empty += DecrementFactor;
            _satiety.ProgressBar.Full += IncrementFactor;
        }

        private void IncrementFactor()
        {
            if (_satiety.ProgressBar.IsEmpty)
                return;

            _factor++;
        }

        private void DecrementFactor()
        {
            if (_isEnableDecrementation)
            {
                if (_factor > 1)
                    _factor -= 1;
            }
        }
    }
}