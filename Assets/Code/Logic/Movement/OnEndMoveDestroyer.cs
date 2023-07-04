using Tools;
using UnityEngine;

namespace Logic.Movement
{
    public class OnEndMoveDestroyer : MonoBehaviour
    {
        [SerializeField] [RequireInterface(typeof(IItemMover))] private MonoBehaviour _mover;

        private IItemMover ItemMover => (IItemMover) _mover;
        
        private void OnEnable()
        {
            ItemMover.Ended += () =>
                Destroy(gameObject);
        }
    }
}