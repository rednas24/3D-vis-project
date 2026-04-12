using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(PlayerMovement3D))]
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private PlayerMovement3D movement;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement3D>();
    }

    private void Update()
    {
        // Movement animation
        animator.SetBool("IsMoving", movement.IsMovingInput);
    }

    // 🎬 Animation triggers

    public void PlayJump()
    {
        animator.SetTrigger("Jump");
    }

    public void PlayAttack()
    {
        animator.SetTrigger("Attack");
    }

    public void PlayDizzy()
    {
        animator.SetTrigger("Dizzy");
    }
}