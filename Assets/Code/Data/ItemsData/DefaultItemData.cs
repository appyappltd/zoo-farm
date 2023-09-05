using UnityEngine;

namespace Data.ItemsData
{
    [CreateAssetMenu(menuName = "Item Data/DefaultItemData", fileName = "NewDefaultItemData", order = 0)]
    public class DefaultItemData : ScriptableObject, IItemData
    {
        [field: SerializeField] public ItemId ItemId { get; private set; }
        [field: SerializeField] public int Weight { get; private set; }
    }
}