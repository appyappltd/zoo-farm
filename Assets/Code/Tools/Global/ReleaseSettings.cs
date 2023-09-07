using Services;
using UnityEngine;

namespace Tools.Global
{
    public class ReleaseSettings : MonoBehaviour
    {
        [SerializeField] private bool _canLetHungryAnimalsRelease;
        
        private IGlobalSettings _settings;

        private void Awake()
        {
            _settings = AllServices.Container.Single<IGlobalSettings>();
            OnValidate();
        }

        private void OnValidate()
        {
            if (Application.isPlaying && _settings is not null)
            {
                _settings.CanLetHungryAnimalsRelease = _canLetHungryAnimalsRelease;
            }
        }
    }
}