using Infrastructure.States;
using Logic;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private LoadingCurtain _curtain;

        private Game _game;

        private void Awake()
        {
            LoadingCurtain curtain = Instantiate(_curtain);
            _game = new Game(this, curtain);
            _game.StateMachine.Enter<BootstrapState>();

#if UNITY_EDITOR
            string sceneName = SceneManager.GetActiveScene().name;
            _game.StateMachine.EditorInitialScene = sceneName;
#endif

            DontDestroyOnLoad(this);
        }
    }
}