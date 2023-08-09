using AYellowpaper;
using Logic.Foods.Vendor;
using Logic.Storages.Items;
using Progress;
using Ui;
using UnityEngine;

namespace Logic.Foods.Carrot
{
    public class Refrigerator : MonoBehaviour, IProgressBarProvider
    {
        [SerializeField] private InterfaceReference<IFoodVendorView, MonoBehaviour> _foodVendor;
        [SerializeField] private BarIconView _barIconView;

        private IProgressBar _progressBarView;

        public IProgressBar ProgressBarView => _progressBarView;

        private void Awake()
        {
            _progressBarView = new ProgressBar(_foodVendor.Value.MaxFoodCount, 0);
            _barIconView.Construct(_progressBarView);
            
            _foodVendor.Value.FoodProduced += OnFoodProduced;
            _foodVendor.Value.Removed += OnRemoved;
        }

        private void OnRemoved(IItem _) =>
            _progressBarView.Spend(1f);

        private void OnFoodProduced(IItem _) =>
            _progressBarView.Replenish(1f);
    }
}