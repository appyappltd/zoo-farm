using DelayRoutines;
using NaughtyAttributes;
using UnityEngine;

namespace Logic.Effects
{
    public class OnRenderEffectShower : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;

        [SerializeField] private bool _isAutoRendererSearch;
        [SerializeField] [HideIf(nameof(_isAutoRendererSearch))] private Renderer _renderer;

        [SerializeField] [MinMaxSlider(0f, 30f)] private Vector2 _randomPreShowDelay = Vector2.one;
        [SerializeField] [Range(0f, 60f)] private float _afterShowDelay = 1f;

        private RoutineSequence _routine;

        private void Awake()
        {
            if (_isAutoRendererSearch)
                _renderer = transform.root.GetComponentInChildren<Renderer>();

            Sisus.Debugging.Debug.Log(_renderer);
            
            _routine = new RoutineSequence(RoutineUpdateMod.FixedRun)
                .WaitUntil(() => _renderer.isVisible)
                .WaitForRandomSeconds(_randomPreShowDelay)
                .Then(_particleSystem.Play)
                .WaitForSeconds(_afterShowDelay)
                .LoopWhile(() => enabled);
        }

        private void Start() =>
            _routine.Play();

        private void OnDestroy()
        {
            Debug.Log("Happy effect kill");
            _routine.Kill();
        }
    }
}