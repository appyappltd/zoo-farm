using System.Collections;
using System.Collections.Generic;
using Data.ItemsData;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Logic.Plant
{
    public class Plant : MonoBehaviour
    {
        public event UnityAction<GameObject> GrowUp = _ => { };

        [SerializeField] private List<GameObject> _stages;
        [SerializeField, Min(.0f)] private float _time;

        private bool canGrow = true;
        private Coroutine _growCoroutine;

        private void Start() =>
            StartGrow();

        [Button("Grow", enabledMode: EButtonEnableMode.Playmode)]
        private void StartGrow()
        {
            _growCoroutine ??= StartCoroutine(Grow());
        }

        private IEnumerator Grow()
        {
            if (canGrow)
            {
                canGrow = false;
                yield return new WaitForSeconds(_time);

                var currStage = Instantiate(_stages[0], transform);
                
                for (int i = 1; i < _stages.Count; i++)
                {
                    yield return new WaitForSeconds(_time);
                    Destroy(currStage);
                    currStage = Instantiate(_stages[i], transform);
                }
                GrowUp.Invoke(currStage);

                var drop = currStage.GetComponent<DropItem>();
                drop.PickedUp += OnPickedUp;
            }
        }

        private void OnPickedUp(HandItem _)
        {
            canGrow = true;
            _growCoroutine = null;
            StartGrow();
        }
    }
}
