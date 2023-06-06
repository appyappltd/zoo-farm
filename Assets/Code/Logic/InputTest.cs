using Services;
using Services.Input;
using UnityEngine;

namespace Logic
{
    public class InputTest : MonoBehaviour
    {
        private IPlayerInputService _input;

        private void Start()
        {
            _input = AllServices.Container.Single<IPlayerInputService>();
        }

        private void Update()
        {
            print(_input.Direction);
        }
    }
}