using Logic.Storages;
using UnityEngine;

namespace Logic
{
    [RequireComponent(typeof(Inventory))]
    [RequireComponent(typeof(Animator))]
    public class HandsAnimator : MonoBehaviour
    {
        private Inventory inventory;
        private Animator animC;

        private void Awake()
        {
            inventory = GetComponent<Inventory>();
            animC = GetComponent<Animator>();

            inventory.Added += _ => ChangeHandsState();
            inventory.Removed += _ => ChangeHandsState();
        }

        private void ChangeHandsState()
        {
            animC.SetBool("Hold", inventory.Weight > 0);
        }
    }
}
