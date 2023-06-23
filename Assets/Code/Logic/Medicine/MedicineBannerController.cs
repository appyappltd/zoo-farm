using UnityEngine;

namespace Logic.Medicine
{
    [RequireComponent(typeof(Inventory.Inventory))]
    public class MedicineBannerController : MonoBehaviour
    {
        [SerializeField] private GameObject _banner;

        private void Awake()
        {
            _banner.SetActive(false);

            var inventory = GetComponent<Inventory.Inventory>();
            inventory.AddItem += _ => _banner.SetActive(true);
            inventory.RemoveItem += _ => _banner.SetActive(false);
        }
    }
}
