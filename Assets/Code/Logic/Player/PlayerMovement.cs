using NTC.Global.Cache;
using Services.Input;
using UnityEngine;

namespace Logic.Player
{
    public class PlayerMovement : MonoCache
    {
        [SerializeField] private ParticleSystem particleS;
        [SerializeField] private HeroAnimator _animator;
        [SerializeField] private float _speed;

        private IPlayerInputService _inputService;
        private Vector3 _moveDirection = Vector3.zero;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _animator.SetSpeed(_speed);
        }

        private void OnValidate()
        {
            _animator.SetSpeed(_speed);
        }

        public void Construct(IPlayerInputService inputService) =>
            _inputService = inputService;

        protected override void Run()
        {
            if (_inputService.Direction.Equals(Vector2.zero))
            {
                _animator.SetMove(false);
                particleS.enableEmission = false;
                return;
            }
        
            _moveDirection = new Vector3(_inputService.Horizontal, _moveDirection.y, _inputService.Vertical);
            _moveDirection = CameraRotation() * _moveDirection;
            _animator.SetMove(true);
            particleS.enableEmission = true;
            var angle = Mathf.Rad2Deg * Mathf.Atan2(_moveDirection.x, _moveDirection.z);
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }

        private Quaternion CameraRotation() =>
            Quaternion.Euler(0, _camera.transform.rotation.eulerAngles.y, 0);
    }
}
