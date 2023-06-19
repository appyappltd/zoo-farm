using System.Collections;
using System.Collections.Generic;
using Data.ItemsData;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Logic.Plant
{
    [RequireComponent(typeof(Delay))]
    public class Plant : MonoBehaviour
    {
        public event UnityAction<GameObject> GrowUp;

        [SerializeField] private List<GameObject> _stages;
        [SerializeField, Min(.0f)] private float _time;
        [SerializeField] private GameObject _sine;

        private bool canGrow = true;

        private void Awake()
            => GetComponent<Delay>().Complete += _ => StartGrow();

        [Button("Grow", enabledMode: EButtonEnableMode.Playmode)]
        private void StartGrow()
        {
            StartCoroutine(Grow());
            _sine.SetActive(false);
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
                GrowUp?.Invoke(currStage);

                var drop = currStage.GetComponent<DropItem>();
                drop.PickUp += _ =>
                {
                    canGrow = true;
                    _sine.SetActive(true);
                };
            }
        }
    }
}
