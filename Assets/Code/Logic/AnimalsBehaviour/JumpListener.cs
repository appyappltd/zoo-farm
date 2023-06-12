using UnityEngine;
using UnityEngine.Events;

namespace Logic.AnimalsBehaviour
{
    public class JumpListener : MonoBehaviour
    {
        [SerializeField] private Jumper _jumper;
        [SerializeField] private UnityEvent _unityEvent;

        private void OnEnable() =>
            _jumper.Jumped += OnJumped;

        private void OnDisable() =>
            _jumper.Jumped += OnJumped;

        private void OnJumped() =>
            _unityEvent?.Invoke();
    }
}