using Progress;
using UnityEngine;

namespace Logic.Animals.AnimalsBehaviour
{
    public class HappinessFactor : MonoBehaviour
    {
        [SerializeField] private bool _isEnableDecrementation;

        private IProgressBarView _satietyBar;
        private int _factor;

        public int Factor => _factor;

        public void Construct(IProgressBarView satietyBar)
        {
            _satietyBar = satietyBar;

            _satietyBar.Empty += DecrementFactor;
            _satietyBar.Full += IncrementFactor;
        }

        private void OnDestroy()
        {
            _satietyBar.Empty += DecrementFactor;
            _satietyBar.Full += IncrementFactor;
        }

        public void ResetFactor()
        {
            _factor = 1;
        }
        
        private void IncrementFactor()
        {
            if (_satietyBar.IsEmpty)
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