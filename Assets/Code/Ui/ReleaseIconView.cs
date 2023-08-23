using Data;
using Logic.Animals;
using Services.Animals;
using Services.StaticData;
using Ui.Elements;
using UnityEngine;

namespace Ui
{
    public class ReleaseIconView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _animalIcon;
        [SerializeField] private TextSetter _amountText;
        
        private AnimalType _actualType;

        private IAnimalCounter _counter;

        private void OnDestroy() =>
            _counter.Updated -= OnCounterUpdated;

        public void Construct(IAnimalsService animalsService, IStaticDataService staticData, AnimalType animalType)
        {
            _actualType = animalType;
            _animalIcon.sprite = staticData.IconByAnimalType(animalType);

            _counter = animalsService.AnimalCounter;
            _counter.Updated += OnCounterUpdated;
            
            _amountText.SetText(0);
        }

        private void OnCounterUpdated(AnimalType type, AnimalCountData countData)
        {
            if (type == _actualType)
                _amountText.SetText(countData.ReleaseReady);
        }
    }
}