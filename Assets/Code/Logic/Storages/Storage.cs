using System;
using System.Collections.Generic;
using Logic.Storages.Items;
using Tools;
using UnityEngine;

namespace Logic.Storages
{
    public class Storage : MonoBehaviour
    {
        private readonly List<IItem> _items = new List<IItem>(10);
        
        [SerializeField] private Transform[] _places;
        
        [SerializeField] [RequireInterface(typeof(IAddItemProvider))] private MonoBehaviour _adderMono;
        [SerializeField] [RequireInterface(typeof(IGetItemProvider))] private MonoBehaviour _removerMono;

        private IAddItemObserver _adder;
        private IGetItemObserver _remover;
        
        private int _topIndex = 0;

        public Action<IItem> Replenished = i => { };

        private IAddItemObserver Adder => _adder;
        // ReSharper disable once SuspiciousTypeConversion.Global
        private IGetItemObserver Remover => _remover;
        
        public Transform TopPlace => _places[_topIndex];

        private IItem TopItem
        {
            get => _items[_topIndex];
            set => _items.Add(value);
        }

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
            Construct((_adderMono as IAddItemProvider)?.AddItemObserver, (_removerMono as IGetItemProvider)?.GetItemObserver);
        }

        public void Construct(IAddItemObserver adder, IGetItemObserver remover)
        {
            _adder = adder;
            _remover = remover;

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
            TopItem = item;
            _topIndex++;
            Replenished.Invoke(item);
        }

        private void RevertItem(IItem item)
        {
            int revertItemIndex = _items.IndexOf(item);
            Sort(revertItemIndex);
            _topIndex--;
        }

        private void Sort(int fromIndex)
        {
            for (int i = fromIndex; i < _items.Count - 1; i++)
            {
                _items[i] = _items[i + 1];
                Transform finishParent = _places[i];
                _items[i].Mover.Move(finishParent, finishParent);
            }
        }
    }
}
