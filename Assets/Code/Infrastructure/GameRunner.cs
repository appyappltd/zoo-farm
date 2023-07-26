using UnityEngine;

namespace Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        [SerializeField] private GameBootstrapper _bootstrapperPrefab;

#if UNITY_EDITOR
        private void Awake()
        {
            var bootstrapper = FindObjectOfType<GameBootstrapper>();

            if (bootstrapper != null)
            {
                return;
            }

            Instantiate(_bootstrapperPrefab);
        }
#endif
    }
}