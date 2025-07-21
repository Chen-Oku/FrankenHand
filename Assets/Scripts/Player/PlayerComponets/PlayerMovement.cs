using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // --- Public References ---
    public CharacterController charController;
    public Transform cameraTransform;
    public Transform spriteVisualTransform;
    public PlayerSoundController soundController;

    // --- Movement Settings ---
    public float speed = 10.0f;
    public float runSpeed = 20.0f;
    public float jumpHeight = 3.5f;
    public float gravity = -58.86f;
    public float dashSpeed = 40f;
    public float dashDuration = 0.2f;
    public float coyoteTime = 2f;


    // --- State Variables ---
    private Vector3 velocity;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool isDashing;
    private float dashTime;
    private float coyoteTimeCounter;
    private bool isRunning = false;
    private Vector3 airMomentum = Vector3.zero;
    private float airControlLerp = 5f;
    private float doubleTapTime = 0.25f;
    private float stepTimer = 0f;
    private float walkStepInterval = 0.4f;
    private float runStepInterval = 0.25f;

    private Dictionary<KeyCode, float> lastTapTimes = new Dictionary<KeyCode, float>
    {
        { KeyCode.W, -1f }, { KeyCode.A, -1f }, { KeyCode.S, -1f }, { KeyCode.D, -1f },
        { KeyCode.UpArrow, -1f }, { KeyCode.LeftArrow, -1f }, { KeyCode.DownArrow, -1f }, { KeyCode.RightArrow, -1f }
    };
    private bool runEnabled = true;
    private float speedMultiplier = 1f;
    private float jumpMultiplier = 1f;
    private bool dashEnabled = true;
    private bool isParalyzed = false;
    private float paralyzeTimer = 0f;
    private int lastFacing = 1; // 1 = derecha, -1 = izquierda
    private bool wasGrounded = true;
    private bool isKnockback = false;
    private float knockbackTimer = 0f;
    private Vector3 knockbackVelocity = Vector3.zero;

    // --- Properties ---
    public bool canMove { get; private set; } = true;
    public bool IsRunning => isRunning;
    public bool IsGrounded => isGrounded;
    public bool IsDashing => isDashing;
    public Vector3 Velocity => velocity;

    // --- Public API ---
    public void SetSpeedMultiplier(float multiplier) => speedMultiplier = multiplier;
    public void SetJumpMultiplier(float multiplier) => jumpMultiplier = multiplier;
    public void DisableDash() => dashEnabled = false;
    public void EnableDash() => dashEnabled = true;
    public void DisableRun() { runEnabled = false; isRunning = false; }
    public void EnableRun() => runEnabled = true;
    public void SetCanMove(bool value) => canMove = value;
    public Vector3 GetVelocity() => velocity;
    public void SetVelocity(Vector3 newVelocity) => velocity = newVelocity;

    public void Paralyze(float duration)
    {
        isParalyzed = true;
        paralyzeTimer = duration;
    }

    public void ResetMovementStateOnRespawn()
    {
        isGrounded = true;
        wasGrounded = true;
        stepTimer = 0f;
        velocity = Vector3.zero;
        airMomentum = Vector3.zero;
        canDoubleJump = true;
        coyoteTimeCounter = coyoteTime;
    }

    // --- Unity Methods ---
    void Awake()
    {
        if (soundController == null)
            soundController = GetComponent<PlayerSoundController>();
    }

    void Update()
    {
        if (!canMove) return;

        if (isParalyzed)
        {
            paralyzeTimer -= Time.deltaTime;
            if (paralyzeTimer <= 0f)
                isParalyzed = false;
            return;
        }

        if (isKnockback)
        {
            charController.Move(knockbackVelocity * Time.deltaTime);
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockback = false;
                knockbackVelocity = Vector3.zero;
            }
            return;
        }

        UpdateGroundedState();

        if (!isDashing)
        {
            HandleMovementInput();
            HandleJumpInput();
        }

        HandleDashInput();
        ApplyGravity();
        charController.Move(velocity * Time.deltaTime);

        if (isGrounded && velocity.magnitude >= 0.1f)
        {
            stepTimer -= Time.deltaTime;
            float interval = isRunning ? runStepInterval : walkStepInterval;
            if (stepTimer <= 0f)
            {
                soundController?.PlayManoCaminando();
                stepTimer = interval;
            }
        }
        else
        {
            stepTimer = 0f;
        }

        // Sonido de aterrizaje
        if (!wasGrounded && isGrounded)
        {
            soundController?.PlayDeGolpeCaidaMano();
        }
        wasGrounded = isGrounded;
    }

    // --- Movement Logic ---
    private void HandleMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (runEnabled)
            DetectDoubleTapForRun();
        else
            isRunning = false;

        float currentSpeed = (isRunning ? runSpeed : speed) * speedMultiplier;

        if (isGrounded)
        {
            if (direction.magnitude >= 0.1f)
            {
                Vector3 moveDir = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * direction;
                charController.Move(moveDir.normalized * currentSpeed * Time.deltaTime);
                airMomentum = moveDir.normalized * currentSpeed;
            }
            else
            {
                isRunning = false;
                airMomentum = Vector3.Lerp(airMomentum, Vector3.zero, 20f * Time.deltaTime);
                if (airMomentum.magnitude < 0.05f)
                    airMomentum = Vector3.zero;
            }
        }
        else
        {
            if (direction.magnitude >= 0.1f)
            {
                Vector3 moveDir = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * direction;
                airMomentum = Vector3.Lerp(airMomentum, moveDir.normalized * currentSpeed, airControlLerp * Time.deltaTime);
            }
            charController.Move(new Vector3(airMomentum.x, 0f, airMomentum.z) * Time.deltaTime);
        }

        // --- Flip tipo Paper Mario ---
        UpdateSpriteDirection(horizontal);
    }

    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || coyoteTimeCounter > 0f)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * jumpMultiplier * -2f * gravity);
                coyoteTimeCounter = 0f;
                canDoubleJump = true; // Permitir doble salto solo después de un salto válido
                soundController?.PlaySaltar();
            }
            else if (canDoubleJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * jumpMultiplier * -2f * gravity);
                canDoubleJump = false;
                soundController?.PlaySaltar();
            }
        }

        if (isGrounded && velocity.y < 0)
        {
            //animator.SetBool("isJumping", false); // Desactiva salto
        }
    }

    private void HandleDashInput()
    {
        if (!dashEnabled)
            return;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetKeyDown(KeyCode.E) && direction.magnitude >= 0.1f && !isDashing)
        {
            StartCoroutine(DashCoroutine(direction));
        }
    }

    private IEnumerator DashCoroutine(Vector3 direction)
    {
        isDashing = true;
        dashTime = dashDuration;

        float targetAngle = cameraTransform.eulerAngles.y;
        Vector3 dashDir = Quaternion.Euler(0f, targetAngle, 0f) * direction;

        while (dashTime > 0)
        {
            charController.Move(dashDir.normalized * dashSpeed * Time.deltaTime);
            dashTime -= Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    private void DetectDoubleTapForRun()
    {
        KeyCode[] keys = { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };
        foreach (var key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                float time = Time.time;
                if (lastTapTimes[key] > 0 && time - lastTapTimes[key] <= doubleTapTime)
                {
                    isRunning = true;
                }
                lastTapTimes[key] = time;
            }
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    private void UpdateGroundedState()
    {
        bool groundedNow = charController.isGrounded;
        isGrounded = groundedNow;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
            canDoubleJump = true;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    // --- Visuals ---
    private void UpdateSpriteDirection(float horizontal)
    {
        if (horizontal > 0.01f)
            lastFacing = 1;
        else if (horizontal < -0.01f)
            lastFacing = -1;

        if (spriteVisualTransform != null)
        {
            float yRot = lastFacing == 1 ? 180f : 0f;
            Quaternion targetFlip = Quaternion.Euler(0f, yRot, 0f);
            spriteVisualTransform.localRotation = Quaternion.Slerp(
                spriteVisualTransform.localRotation,
                targetFlip,
                10f * Time.deltaTime // Ajusta la velocidad aquí para más o menos suavidad
            );
        }
    }

    // --- Respawn ---
    public void RespawnPlayer()
    {
        ResetMovementStateOnRespawn();
        isGrounded = true;
        wasGrounded = true;
        stepTimer = 0f;
        velocity = Vector3.zero;
        airMomentum = Vector3.zero;
        canDoubleJump = true;
        coyoteTimeCounter = coyoteTime;
        // ...other respawn logic...
    }

    public void ApplyKnockback(Vector3 direction, float force, float duration)
    {
        isKnockback = true;
        knockbackTimer = duration;
        knockbackVelocity = direction.normalized * force;
    }
}
