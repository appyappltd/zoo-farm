using System;
using Logic.Storages.Items;
using Logic.Translators;
using NaughtyAttributes;
using Tools;
using UnityEngine;

namespace Logic.Storages
{
    public class Storage : MonoBehaviour
    {
        private readonly IItem[] _items = new IItem[20];

        [SerializeField] private Transform[] _places;

        [SerializeField] private bool _isSortable;
        [SerializeField] private bool _isModifyRotation;
        [SerializeField] private bool _isSupportAnimated;
        [SerializeField] private bool _isRemoteInit;

        [HideIf("_isRemoteInit")] [SerializeField] [RequireInterface(typeof(IAddItemObserver))]
        private MonoBehaviour _adderMono;

        [HideIf("_isRemoteInit")] [SerializeField] [RequireInterface(typeof(IGetItemObserver))]
        private MonoBehaviour _removerMono;

        private IAddItemObserver _adder;
        private IGetItemObserver _remover;

        private Action<IItem> MoveItem;
        private ITranslator _translator;
        private int _topIndex = -1;

        public event Action<IItem> Replenished = _ => { };

        private IAddItemObserver Adder => _adder;
        private IGetItemObserver Remover => _remover;

        public Transform TopPlace => _places[Mathf.Max(0, _topIndex)];

        private void Awake()
        {
            if (_isSupportAnimated)
            {
                _translator = gameObject.AddComponent<RunTranslator>();
                MoveItem = MoveItemWithCallback;
            }
            else
            {
                MoveItem = MoveItemNoneCallback;
            }
            
            enabled = !_isRemoteInit;
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
            Replenished.Invoke(item);
        }

        private void RevertItem(IItem item)
        {
            if (_isSortable)
            {
                int revertItemIndex = Array.IndexOf(_items, item);
                Sort(revertItemIndex);
            }

            if (_isSupportAnimated)
                StopAnimation(item);

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

        private void StopAnimation(IItem item)
        {
            Debug.Log("StopAnimation");
            
            if (item is not ITranslatableAnimated animated)
                return;
            
            foreach (ITranslatableParametric<Vector3> translatable in animated.GetAllTranslatables())
            {
                if (translatable.IsPreload == false)
                    continue;
                
                translatable.Stop(false);
            }
        }
        
        private void StartAnimation(IItem item)
        {
            Debug.Log("StartAnimation");
            
            if (item is not ITranslatableAnimated animated)
                return;

            foreach (ITranslatableParametric<Vector3> translatable in animated.GetAllTranslatables())
            {
                if (translatable.IsPreload)
                {
                    translatable.Play();
                    _translator.Add(translatable);
                }
            }
        }

        private void MoveItemNoneCallback(IItem item) =>
            item.Mover.Move(TopPlace, TopPlace, _isModifyRotation);

        private void MoveItemWithCallback(IItem item)
        {
            void Proxy() => StartAnimation(item);
            item.Mover.Move(TopPlace, Proxy, TopPlace, _isModifyRotation);
        }
    }
}