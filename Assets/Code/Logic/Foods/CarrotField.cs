using Logic.Storages.Items;
using NTC.Global.System;
using UnityEngine;

namespace Logic.Foods
{
    [RequireComponent(typeof(FoodVendor))]
    public class CarrotField : MonoBehaviour
    {
        [SerializeField] private FoodVendor _foodVendor;
        [SerializeField] private GameObject _sprout;
        
        private void Awake()
        {
            _foodVendor.BeginProduceFood += OnBeginProduceFood;
            _foodVendor.FoodProduced += OnFoodProduced;
        }

        private void OnFoodProduced(IItem item) =>
            _sprout.Disable();

        private void OnBeginProduceFood() =>
            _sprout.Enable();
    }
}