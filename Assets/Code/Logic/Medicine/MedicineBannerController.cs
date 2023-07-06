using Logic.Storages;
using UnityEngine;

namespace Logic.Medicine
{
    public class MedicineBannerController : MonoBehaviour
    {
        [SerializeField] private GameObject _banner;

        // private void Awake()
        // {
        //     _banner.SetActive(false);
        //
        //     var inventory = GetComponent<Inventory>();
        //     inventory.Added += _ => _banner.SetActive(true);
        //     inventory.Removed += _ => _banner.SetActive(false);
        // }
    }
}
