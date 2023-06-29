using Tools;
using UnityEngine;
using UnityEngine.Events;

namespace Logic.Movement
{
    public class OnEndMoveDestroyer : MonoBehaviour
    {
        [SerializeField] [RequireInterface(typeof(IMover))] private MonoBehaviour _mover;

        private IMover Mover => (IMover) _mover;
        
        private void OnEnable()
        {
            Mover.GotToPlace += () => Destroy(gameObject);
        }
    }
}