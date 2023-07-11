using System;
using Logic.Storages.Items;
using NaughtyAttributes;
using Tools;
using UnityEngine;

namespace Logic.Storages
{
    public class Storage : MonoBehaviour
    {
        private readonly IItem[] _items = new IItem[10];
        
        [SerializeField] private Transform[] _places;

        [SerializeField] private bool _isSortable;
        [SerializeField] private bool _isRemoteInit;
        
        [HideIf("_isRemoteInit")]
        [SerializeField] [RequireInterface(typeof(IAddItemObserver))] private MonoBehaviour _adderMono;
        [HideIf("_isRemoteInit")]
        [SerializeField] [RequireInterface(typeof(IGetItemObserver))] private MonoBehaviour _removerMono;

        private IAddItemObserver _adder;
        private IGetItemObserver _remover;
        
        private int _topIndex = 0;

        public Action<IItem> Replenished = i => { };

        private IAddItemObserver Adder => _adder;
        // ReSharper disable once SuspiciousTypeConversion.Global
        private IGetItemObserver Remover => _remover;
        
        public Transform TopPlace => _places[_topIndex];

        private void Awake() =>
            enabled = !_isRemoteInit;

        private void OnDestroy()
        {
            if (Adder is not null)
            {
                Adder.Added -= PlaceItem;
            }
            
            if (Remover is not null)
            {
                Remover.Removed -= RevertItem;
            }
        }

        private void OnEnable()
        {
            if (_isRemoteInit)
            {
                enabled = false;
                return;
            }
            
            Construct(_adderMono as IAddItemObserver, _removerMono as IGetItemObserver);
        }

        public void Construct(IAddItemObserver adder, IGetItemObserver remover)
        {
            _adder = adder;
            _remover = remover;

            Subscribe();
        }
        
        public void Construct(IInventory inventory)
        {
            _adder = inventory;
            _remover = inventory;

            Subscribe();
        }

        private void Subscribe()
        {
            if (Adder is not null)
            {
                Adder.Added += PlaceItem;
            }
            
            if (Remover is not null)
            {
                Remover.Removed += RevertItem;
            }
        }

        private void PlaceItem(IItem item)
        {
            item.Mover.Move(TopPlace, TopPlace);
            _items[_topIndex] = item;
            Debug.Log("place" + " " + gameObject.name + " index " + _topIndex);
            _topIndex++;
            Replenished.Invoke(item);
        }

        private void RevertItem(IItem item)
        {
            int revertItemIndex = Array.IndexOf(_items, item);
            Debug.Log("revert" + " " + gameObject.name + " index " + revertItemIndex);
            Sort(revertItemIndex);
            _topIndex--;
        }

        private void Sort(int fromIndex)
        {
            if (_isSortable == false)
                return;

            for (int i = fromIndex; i < _topIndex - 1; i++)
            {
                _items[i] = _items[i + 1];
                Transform finishParent = _places[i];
                _items[i].Mover.Move(finishParent, finishParent);
                Debug.Log($"Sort {i}");
            }
        }
    }
}
