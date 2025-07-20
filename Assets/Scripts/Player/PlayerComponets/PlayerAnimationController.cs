using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator animator;
    private PlayerMovement movement;

    private bool estabaEmpujando = false;

    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    public void UpdateMovementAnimations(float horizontal, float vertical, bool isRunning)
    {
        float moveAmount = new Vector2(horizontal, vertical).magnitude;
        float animSpeed = moveAmount > 0 ? (isRunning ? 1f : 0.5f) : 0f;
        animator.SetFloat("idleWaling", animSpeed);
    }

    public void UpdateJumpAnimation(bool isJumping)
    {
        animator.SetBool("isJumping", isJumping);
    }

    public void UpdateDashAnimation(bool isDashing)
    {
        animator.SetBool("isDashing", isDashing);
    }

    public void UpdateYVelocity(float yVelocity)
    {
        animator.SetFloat("yVelocity", yVelocity);
    }

    public void UpdatePushAnimations(bool estaAgarrando, float pushSpeed, bool estaEmpujando)
    {
        animator.SetFloat("pushSpeed", pushSpeed);

        if (estaAgarrando)
        {
            if (!estabaEmpujando && estaEmpujando)
                animator.SetTrigger("startPush");

            animator.SetBool("isPushing", estaEmpujando);
            animator.SetBool("isHoldingPush", !estaEmpujando);

            estabaEmpujando = estaEmpujando;
        }
        else
        {
            animator.SetFloat("pushSpeed", 0f);
            animator.SetBool("isPushing", false);
            animator.SetBool("isHoldingPush", false);
            estabaEmpujando = false;
        }
    }

    public void TriggerTakeDamage()
    {
        animator?.SetTrigger("TakeDamage");
    }

    public void ForzarIdle()
    {
        if (animator != null)
            animator.SetFloat("idleWaling", 0f); // O usa animator.Play("Idle") si prefieres
    }

    void Update()
    {
        if (!movement.canMove) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        UpdateMovementAnimations(horizontal, vertical, movement.IsRunning);
        UpdateJumpAnimation(!movement.IsGrounded);
        UpdateDashAnimation(movement.IsDashing);
        UpdateYVelocity(movement.Velocity.y);
        // Si tienes lógica de empujar aquí, llama también a UpdatePushAnimations
    }
}