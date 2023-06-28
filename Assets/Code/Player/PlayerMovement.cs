using NTC.Global.Cache;
using Services.Input;
using UnityEngine;

public class PlayerMovement : MonoCache
{
    [SerializeField] private ParticleSystem particleS;

    private IPlayerInputService _inputService;
    private Animator animator;
    private Vector3 moveDirection = Vector3.zero;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        animator = GetComponent<Animator>();
    }

    public void Construct(IPlayerInputService inputService) =>
        _inputService = inputService;

    protected override void FixedRun()
    {
        if (_inputService.Direction.Equals(Vector2.zero))
        {
            SetAnimatorMove(false);
            particleS.enableEmission = false;
            return;
        }
        
        moveDirection = new Vector3(_inputService.Horizontal, moveDirection.y, _inputService.Vertical);
        moveDirection = CameraRotation() * moveDirection;
        SetAnimatorMove(true);
        particleS.enableEmission = true;
        var angle = Mathf.Rad2Deg * Mathf.Atan2(moveDirection.x, moveDirection.z);
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void SetAnimatorMove(bool isMoving)
    {
        animator.SetBool("Moving", isMoving);
    }

    private Quaternion CameraRotation() =>
        Quaternion.Euler(0, _camera.transform.rotation.eulerAngles.y, 0);
}
