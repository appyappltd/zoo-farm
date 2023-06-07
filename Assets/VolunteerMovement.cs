using UnityEngine;

public class VolunteerMovement : MonoBehaviour
{
    private Animator animC;
    private Mover mover;

    private void Awake()
    {
        animC = GetComponent<Animator>();
        mover = GetComponent<Mover>();
        mover.StartMove += OnMove;
        mover.GotToPlace += OnMove;
    }

    private void OnMove() => animC.SetBool("IsMoving", mover.IsMoving);
}
