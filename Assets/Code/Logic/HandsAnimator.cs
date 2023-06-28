using UnityEngine;

namespace Logic
{
    [RequireComponent(typeof(Inventory.Inventory))]
    [RequireComponent(typeof(Animator))]
    public class HandsAnimator : MonoBehaviour
    {
        private Inventory.Inventory inventory;
        private Animator animC;
        public  bool Debug;

        private void Awake()
        {
            inventory = GetComponent<Inventory.Inventory>();
            animC = GetComponent<Animator>();

            inventory.AddItem += _ => ChangeHandsState();
            inventory.RemoveItem += _ => ChangeHandsState();
        }

        private void ChangeHandsState()
        {
            animC.SetBool("Hold", inventory.GetCount > 0);
        }
    }
}
