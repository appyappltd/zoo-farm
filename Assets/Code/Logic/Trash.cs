using System;
using Logic.Storages;
using Logic.Storages.Items;
using UnityEngine;

namespace Logic
{
    public class Trash : MonoBehaviour, IAddItem
    {
        public event Action<IItem> Added = _ => { };

        public void Add(IItem item)
        {
            Added.Invoke(item);
            item.Mover.Move(transform, item.Destroy, null, false, true);
        }

        public bool CanAdd(IItem item) =>
            true;

        public bool TryAdd(IItem item)
        {
            if (CanAdd(item))
            {
                Add(item);
                return true;
            }

            return false;
        }
    }
}