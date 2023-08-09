using AYellowpaper;
using Logic.Foods.Vendor;
using Logic.Storages.Items;
using NTC.Global.System;
using UnityEngine;

namespace Logic.Foods.Carrot
{
    [RequireComponent(typeof(FoodVendor))]
    public class CarrotField : MonoBehaviour
    {
        [SerializeField] private InterfaceReference<IFoodVendorView, MonoBehaviour> _foodVendor;
        [SerializeField] private GameObject _sprout;
        
        private void Awake()
        {
            _foodVendor.Value.BeginProduceFood += OnBeginProduceFood;
            _foodVendor.Value.FoodProduced += OnFoodProduced;
        }

        private void OnFoodProduced(IItem item) =>
            _sprout.Disable();

        private void OnBeginProduceFood() =>
            _sprout.Enable();
    }
}