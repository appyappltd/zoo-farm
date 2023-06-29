using System.Collections;
using UnityEngine;

namespace Logic.Volunteer
{
    public class Volunteer : MonoBehaviour
    {
        [HideInInspector] public bool CanGiveAnimal = false;
        [HideInInspector] public bool CanTakeAnimal = false;

        [SerializeField] private Vector2 _reloadTime = new(5, 45);

        public void Reload() =>
            StartCoroutine(ReloadCor());

        public IEnumerator ReloadCor()
        {
            var time = Random.Range(_reloadTime.x, _reloadTime.y);
            yield return new WaitForSeconds(time);
            CanTakeAnimal = true;
        }
    }
}