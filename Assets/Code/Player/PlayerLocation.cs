using Data;
using Services.PersistentProgress;
using Tools.Extension;
using UnityEngine;

namespace Player
{
    public class PlayerLocation : MonoBehaviour, ISavedProgress
    {
        public void LoadProgress(PlayerProgress progress)
        {
            CharacterController controller = GetComponent<CharacterController>();
            controller.enabled = false;
            transform.position = progress.LevelData.PlayerLocationData.Position.AsUnityVector();
            transform.rotation = Quaternion.Euler(progress.LevelData.PlayerLocationData.Rotation.AsUnityVector());
            controller.enabled = true;
        }

        public void UpdateProgress(PlayerProgress progress)
        {
            progress.LevelData.PlayerLocationData.Position = transform.position.AsVectorData();
            progress.LevelData.PlayerLocationData.Rotation = transform.rotation.eulerAngles.AsVectorData();
        }
    }
}