using Data;
using Services.PersistentProgress;
using Services.SaveLoad;
using Tools.Extension;
using UnityEngine;

namespace Player
{
    public class PlayerLocation : MonoBehaviour, ISavedProgressGeneric
    {
        public void LoadProgress(IPersistentProgressService progressService)
        {
            var progress = progressService.GetData<PlayerLocationData>();
            
            var controller = GetComponent<CharacterController>();
            controller.enabled = false;
            Debug.Log(progress);
            transform.position = progress.Position.AsUnityVector();
            transform.rotation = Quaternion.Euler(progress.Rotation.AsUnityVector());
            Debug.Log($"Load {progress.Position.AsUnityVector()}");
            controller.enabled = true;
        }

        public void UpdateProgress(IPersistentProgressService progressService)
        {
            var progress = progressService.GetData<PlayerLocationData>();
            
            Debug.Log(progress);
            progress.Position = transform.position.AsVectorData();
            progress.Rotation = transform.rotation.eulerAngles.AsVectorData();
            Debug.Log($"Update {transform.position}");
        }
    }
}