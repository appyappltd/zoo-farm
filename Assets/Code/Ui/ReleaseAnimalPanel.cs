using System.Collections.Generic;
using System.Diagnostics;
using Logic.Animals;
using NaughtyAttributes;
using Observables;
using Services.Animals;
using Tools;
using Ui.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class ReleaseAnimalPanel : MonoBehaviour
    {
        private readonly Observable<int> _releaseAnimalCount = new Observable<int>();
        
        [SerializeField] private AnimalType _animalType;
        [SerializeField] private TextSetter _animalsCount;
        [SerializeField] private TextSetter _toReleaseCount;
        [SerializeField] private Button _increaseCounterButton;
        [SerializeField] private List<UiGrayScaleFilter> _grayScaleFilters;
        
        
#if UNITY_EDITOR
        [SerializeField] private int _testTotal;
        [SerializeField] private int _testReady;
#endif

        private AnimalCountData _countData;

        public IObservable<int> ReleaseAnimalCount => _releaseAnimalCount;

        public AnimalType AnimalType => _animalType;
        
        public void Construct(AnimalCountData countData)
        {
            _countData = countData;
            _animalsCount.SetText($"{countData.ReleaseReady}/{countData.Total}");
            _increaseCounterButton.onClick.AddListener(IncreaseReleaseCounter);

            if (countData.ReleaseReady == 0)
            {
                _increaseCounterButton.interactable = false;

                for (var index = 0; index < _grayScaleFilters.Count; index++)
                {
                    UiGrayScaleFilter filter = _grayScaleFilters[index];
                    filter.SetEffectAmount(1);
                }
            }
            
            gameObject.SetActive(true);
        }

        private void IncreaseReleaseCounter()
        {
            _releaseAnimalCount.Value++;
            _toReleaseCount.SetText(_releaseAnimalCount.Value);
            
            if (_releaseAnimalCount.Value >= _countData.ReleaseReady)
            {
                _increaseCounterButton.interactable = false;
                _increaseCounterButton.onClick.RemoveListener(IncreaseReleaseCounter);
            }
        }

        [Button("Set Test Count")] [Conditional("UNITY_EDITOR")]
        private void SetTestCount()
        {
            Construct(new AnimalCountData(_testTotal, _testReady));
        }
    }
}