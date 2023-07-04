using UnityEngine;

namespace Data.ItemsData
{
    [CreateAssetMenu(fileName = "Item Data", menuName = "itemData")]
    public class ItemData : ScriptableObject
    {
        [field: SerializeField] public HandItem Hand { get; private set; }
        [field: SerializeField] public DropItem Drop { get; private set; }
        [field: SerializeField] public ItemId Creature { get; private set; }
    }
}
