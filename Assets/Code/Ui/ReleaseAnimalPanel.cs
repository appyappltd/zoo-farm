using System.Diagnostics;
using Logic.Animals;
using NaughtyAttributes;
using Observables;
using Services.Animals;
using Ui.Elements;
using UnityEngine;
using UnityEngine.UI;

namespace Ui
{
    public class ReleaseAnimalPanel : MonoBehaviour
    {
        [SerializeField] private AnimalType _animalType;
        [SerializeField] private TextSetter _animalsCount;
        [SerializeField] private Button _increaseCounterButton;
        
#if UNITY_EDITOR
        [SerializeField] private int _testTotal;
        [SerializeField] private int _testReady;
#endif

        private AnimalCountData _countData;
        private Observable<int> _releaseAnimalCount;

        public IObservable<int> ReleaseAnimalCount => _releaseAnimalCount;

        public AnimalType AnimalType => _animalType;
        
        public void Construct(AnimalCountData countData)
        {
            _countData = countData;
            _releaseAnimalCount = new Observable<int>();
            _animalsCount.SetText($"{countData.ReleaseReady}/{countData.Total}");
            _increaseCounterButton.onClick.AddListener(IncreaseReleaseCounter);
            gameObject.SetActive(true);
        }

        private void IncreaseReleaseCounter()
        {
            if (_releaseAnimalCount.Value < _countData.Total)
            {
                _releaseAnimalCount.Value++;
            }
            else
            {
                _increaseCounterButton.interactable = false;
            }
        }

        [Button("Set Test Count")] [Conditional("UNITY_EDITOR")]
        private void SetTestCount()
        {
            Construct(new AnimalCountData(_testTotal, _testReady));
        }
    }
}