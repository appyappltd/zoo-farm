using Tutorial.StaticTriggers;
using UnityEngine;

namespace Logic
{
    public class ChildCountObserver : MonoBehaviour
    {
        [SerializeField] private TutorialTriggerStatic _hasNoChild;

        private void OnTransformChildrenChanged()
        {
            if (transform.childCount <= 0)
            {
                _hasNoChild.Trigger(gameObject);
                Debug.Log("child zero");
            }
        }
    }
}