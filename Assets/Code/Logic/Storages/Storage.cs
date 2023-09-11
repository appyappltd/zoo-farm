using System;
using AYellowpaper;
using Logic.Storages.Items;
using Logic.Translators;
using NaughtyAttributes;
using UnityEngine;

namespace Logic.Storages
{
    public class Storage : MonoBehaviour
    {
        private readonly IItem[] _items = new IItem[20];

        [SerializeField] private Transform[] _places;

        [SerializeField] private bool _isSortable;
        [SerializeField] private bool _isModifyRotation;
        [SerializeField] private bool _isRemoteInit;

        [HideIf(nameof(_isRemoteInit))] [SerializeField] [Tools.RequireInterface(typeof(IAddItemObserver))]
        private MonoBehaviour _adderMono;

        [HideIf(nameof(_isRemoteInit))] [SerializeField] [Tools.RequireInterface(typeof(IGetItemObserver))]
        private MonoBehaviour _removerMono;

        [SerializeField] private bool _isSupportAnimated;
        [SerializeField] [ShowIf(nameof(_isSupportAnimated))] private InterfaceReference<ITranslator, MonoBehaviour> _translator;
        
        private IAddItemObserver _adder;
        private IGetItemObserver _remover;

        private Action<IItem> MoveItem;
        private int _topIndex = -1;

        private IAddItemObserver Adder => _adder;
        private IGetItemObserver Remover => _remover;

        public Transform TopPlace => _places[Mathf.Max(0, _topIndex)];

        private void Awake()
        {
            if (_isSupportAnimated)
                MoveItem = MoveItemWithCallback;
            else
                MoveItem = MoveItemNoneCallback;

            enabled = !_isRemoteInit;
        }

        private void OnDestroy()
        {
            if (Adder is not null)
                Adder.Added -= PlaceItem;

            if (Remover is not null)
                Remover.Removed -= RevertItem;
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
            _topIndex++;
            MoveItem.Invoke(item);
            _items[_topIndex] = item;
        }

        private void RevertItem(IItem item)
        {
            if (_isSortable)
            {
                int revertItemIndex = Array.IndexOf(_items, item);
                Sort(revertItemIndex);
            }

            if (_isSupportAnimated)
                StopAnimation(_topIndex);

            _topIndex--;
        }

        private void Sort(int fromIndex)
        {
            for (int i = fromIndex; i < _topIndex - 1; i++)
            {
                _items[i] = _items[i + 1];
                Transform finishParent = _places[i];
                _items[i].Mover.Move(finishParent, finishParent);
            }
        }

        private void StopAnimation(int placeIndex)
        {
            var translatables = _places[placeIndex].GetComponents<ITranslatableParametric<Vector3>>();
            
            foreach (ITranslatableParametric<Vector3> translatable in translatables)
            {
                if (translatable.IsPreload == false)
                    continue;
                
                translatable.Stop(false);
                translatable.ResetToDefault();
            }
        }
        
        private void StartAnimation(int placeIndex)
        {
            var translatables = _places[placeIndex].GetComponents<ITranslatableParametric<Vector3>>();
            
            foreach (ITranslatableParametric<Vector3> translatable in translatables)
            {
                if (translatable.IsPreload)
                {
                    translatable.Play();
                    _translator.Value.Add(translatable);
                }
            }
        }

        private void MoveItemNoneCallback(IItem item) =>
            item.Mover.Move(TopPlace, TopPlace, _isModifyRotation);

        private void MoveItemWithCallback(IItem item)
        {
            int topIndex = _topIndex;
            item.Mover.Move(TopPlace, () => StartAnimation(topIndex), TopPlace, _isModifyRotation);
        }
    }
}