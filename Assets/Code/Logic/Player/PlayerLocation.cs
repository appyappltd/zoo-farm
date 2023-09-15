using Data.SaveData;
using Services.PersistentProgressGeneric;
using Tools.Extension;
using UnityEngine;

namespace Logic.Player
{
    public class PlayerLocation : MonoBehaviour, ISavedProgressGeneric<PlayerLocationData>
    {
        public void LoadProgress(in PlayerLocationData progress)
        {
            CharacterController controller = GetComponent<CharacterController>();
            controller.enabled = false;
            transform.position = progress.Position.AsUnityVector();
            transform.rotation = Quaternion.Euler(progress.Rotation.AsUnityVector());
            controller.enabled = true;
        }

        public void UpdateProgress(ref PlayerLocationData progress)
        {
            progress.Position = transform.position.AsVectorData();
            progress.Rotation = transform.rotation.eulerAngles.AsVectorData();
        }
    }
}