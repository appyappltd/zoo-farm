using Logic.Bubble;
using Logic.Storages;
using UnityEngine;

namespace Logic
{
    public class HeroConstructor : MonoBehaviour
    {
        [SerializeField] private HeroProvider _provider;
        [SerializeField] private Storage _storage;
        [SerializeField] private HandsAnimator _handsAnimator;
        [SerializeField] private MaxIndicator _maxIndicator;

        private void Awake()
        {
            _storage.Construct(_provider.Inventory, _provider.Inventory);
            _handsAnimator.Construct(_provider.Inventory);
            _maxIndicator.Construct(_provider.Inventory);
        }
    }
}