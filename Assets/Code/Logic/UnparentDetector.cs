using UnityEngine;
using UnityEngine.Events;

namespace Logic
{
    public class UnparentDetector : MonoBehaviour
    {
        [SerializeField] private UnityEvent OnUnparented;

        private void OnTransformParentChanged()
        {
            if (transform.parent is null)
                OnUnparented.Invoke();
        }
    }
}