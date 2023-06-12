using UnityEngine;

namespace Data.ItemsData
{
    public class DropItem : MonoBehaviour
    {
        [field: SerializeField] public ItemData ItemData { get; private set; }
    }
}
