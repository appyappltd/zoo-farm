using System.Collections;
using Infrastructure;
using Services;
using UnityEngine;

namespace Logic
{
    public class TestCoroutineRunner : MonoBehaviour
    {
        private void Awake()
        {
            AllServices.Container.Single<ICoroutineRunner>().StartCoroutine(Test());
        }

        private IEnumerator Test()
        {
            yield return null;
        }
    }
}
