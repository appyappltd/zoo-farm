using UnityEngine;

namespace Data.ItemsData
{
    public class HandItem : MonoBehaviour
    {
        [field: SerializeField] public ItemData ItemData { get; private set; }
        [field: SerializeField] public Transform NextPlace { get; private set; }
    }
}
