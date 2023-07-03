using System;
using System.Collections.Generic;
using System.Linq;
using Data.ItemsData;
using UnityEngine;
using UnityEngine.Events;

namespace Logic.Inventory
{
    [RequireComponent(typeof(Storage))]
    public class Inventory : MonoBehaviour
    {
        public event UnityAction<HandItem> AddItem = c => { };
        public event UnityAction<HandItem> RemoveItem = c => { };

        public bool IsMax => Weight == MaxWeight;
        public HandItem GetLast => items.Last();
        public HandItem GetPreLast => items[^2];
        public int GetCount => items.Count;
        public CreatureType Type => currType;
        public ItemData GetData => items.Last().ItemData;
        [field: SerializeField, Min(0)] public int MaxWeight { get; private set; } = 3;
        public int Weight { get; private set; } = 0;

        [SerializeField] private CreatureType _type = CreatureType.None;
        [SerializeField] private List<HandItem> items = new();//Debag  [SerializeField] 

        private CreatureType currType = CreatureType.None;

        private void Awake()
        {
            currType = _type;
        }

        public bool CanAddItem(CreatureType type, int weight) => MaxWeight >= Weight + weight
                                                                 && (currType == CreatureType.None
                                                                     || currType == type);
        public bool CanGiveItem(CreatureType type) => items.Count > 0
                                                      && currType == type;
        public bool CanGiveItem() => items.Count > 0;

        public void Add(HandItem item)
        {
            if (items.Contains(item))
                throw new ArgumentException();
            if (currType == CreatureType.None)
                currType=item.ItemData.Creature;

            items.Add(item);
            Weight += item.Weight;

            AddItem.Invoke(item);
        }

        public HandItem Remove()
        {
            if (items.Count < 1)
                throw new ArgumentNullException();
            var item = items.Last();

            items.Remove(item);
            Weight -= item.Weight;

            RemoveItem.Invoke(item);

            if (items.Count == 0)
                currType=_type;
            return item;
        }
    }
}
