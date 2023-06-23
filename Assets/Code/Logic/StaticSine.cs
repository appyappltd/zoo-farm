using System.Collections;
using Logic.Interactions;
using NaughtyAttributes;
using UnityEngine;

namespace Logic
{
    public class StaticSine : MonoBehaviour
    {
        [SerializeField] private Vector3 _targetSize;
        [SerializeField, Min(.0f)] private float _speed =.1f;
        [SerializeField, Min(.0f)] private float _offset = .1f;
        [SerializeField] private TriggerObserver _trigger;

        private Vector3 defSize;
        private Coroutine coroutine;
        private Vector3 lastSc = new(121,432,543);

        private void Awake()
        {
            defSize = transform.localScale;

            _trigger.Enter += _ => OnInteractable(_targetSize);
            _trigger.Exit += _ => OnInteractable(defSize);
        }

        [Button("расти", enabledMode: EButtonEnableMode.Playmode)]
        public void A() => OnInteractable(_targetSize);

        [Button("Уменьшайся", enabledMode: EButtonEnableMode.Playmode)]
        public void B() => OnInteractable(defSize);

        public void OnInteractable(Vector3 size)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = StartCoroutine(ChangeSize(size));
        }

        private IEnumerator ChangeSize(Vector3 targetSize)
        {
            while ((transform.localScale - targetSize).magnitude > _offset 
                   && lastSc != transform.localScale)
            {
                lastSc = transform.localScale;
                transform.localScale = Vector3.MoveTowards(transform.localScale,
                    targetSize,
                    _speed * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
