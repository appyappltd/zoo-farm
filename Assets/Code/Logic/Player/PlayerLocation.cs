using Data.SaveData;
using Services;
using Services.PersistentProgressGeneric;
using Services.SaveLoad;
using Tools.Extension;
using UnityEngine;

namespace Logic.Player
{
    public class PlayerLocation : MonoBehaviour, ISavedProgressGeneric<PlayerLocationData>
    {
        private void Awake()
        {
            Construct(AllServices.Container.Single<ISaveLoadService>());
        }

        private void Construct(ISaveLoadService saveLoad) =>
            saveLoad.Register<PlayerLocationData>(this);

        public void LoadProgress(in PlayerLocationData progress)
        {
            CharacterController controller = GetComponent<CharacterController>();
            controller.enabled = false;
            transform.position = progress.Position.AsUnityVector();
            transform.rotation = Quaternion.Euler(progress.Rotation.AsUnityVector());
            controller.enabled = true;
        }

        public void UpdateProgress(PlayerLocationData progress)
        {
            progress.Position = transform.position.AsVectorData();
            progress.Rotation = transform.rotation.eulerAngles.AsVectorData();
        }
    }
}