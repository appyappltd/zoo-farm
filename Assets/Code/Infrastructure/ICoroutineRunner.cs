using System.Collections;
using Services;
using UnityEngine;

namespace Infrastructure
{
  public interface ICoroutineRunner : IService
  {
    Coroutine StartCoroutine(IEnumerator coroutine);
    void StopCoroutine(Coroutine routine);
  }
}