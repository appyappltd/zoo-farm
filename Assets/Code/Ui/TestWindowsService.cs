using NaughtyAttributes;
using Services;
using Ui.Services;
using UnityEngine;

namespace Ui
{
    public class TestWindowsService : MonoBehaviour
    {
        private IWindowService _windowService;
        
        private void Awake()
        {
            _windowService = AllServices.Container.Single<IWindowService>();
        }

        [Button("Open Release window")]
        private void OpenReleaseWindow()
        {
            _windowService.Open(WindowId.AnimalRelease);
        }
    }
}