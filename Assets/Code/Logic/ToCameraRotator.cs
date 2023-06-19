using UnityEngine;

namespace Logic
{
    public class ToCameraRotator : MonoBehaviour
    {
        private void Awake() =>
            transform.forward = Camera.main.transform.forward;
    }
}