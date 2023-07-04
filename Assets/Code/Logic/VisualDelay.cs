using Tools.Extension;
using UnityEngine;

namespace Logic
{
    public class VisualDelay : MonoBehaviour
    {
        [SerializeField] private GameObject _fill;
        [SerializeField] private float startPoint = -4.9f;

        private float endPoint = 0;
        private float currY = 0;
        private float distance;

        private void Awake()
        {
            _fill.transform.localScale.ChangeY(startPoint);
            distance = endPoint - startPoint;
        }

        private void OnChanged(float max, float amount)
        {
            currY = Mathf.Clamp(-(distance - distance * amount / max), startPoint, endPoint);
            _fill.transform.localPosition = _fill.transform.localPosition.ChangeY(currY);
        }
    }
}
