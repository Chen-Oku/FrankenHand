using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController charController;
    public Transform cameraTransform;
    public Transform spriteTransform; // Asigna el objeto del sprite en el Inspector
    public float speed = 6.0f;
    public float runSpeed = 12.0f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float coyoteTime = 0.2f;

    private Vector3 velocity;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool isDashing;
    private float dashTime;
    private float coyoteTimeCounter;
    private Vector3 lastDirection = Vector3.zero;
    private float doubleTapTime = 0.25f; // Tiempo máximo entre toques
    private Dictionary<KeyCode, float> lastTapTimes = new Dictionary<KeyCode, float>
    {
        { KeyCode.W, -1f },
        { KeyCode.A, -1f },
        { KeyCode.S, -1f },
        { KeyCode.D, -1f },
        { KeyCode.UpArrow, -1f },
        { KeyCode.LeftArrow, -1f },
        { KeyCode.DownArrow, -1f },
        { KeyCode.RightArrow, -1f }
    };
    private bool isRunning = false;
    private Vector3 airMomentum = Vector3.zero;
    private float airControlLerp = 5f; // Controla qué tan rápido cambia el momentum en el aire

    public Checkpoint lastCheckpoint; // Referencia al último checkpoint

    private Quaternion targetSpriteRotation = Quaternion.Euler(90f, 180f, 0f); // Agrega este campo arriba

    void Start()
    {
        charController = charController ?? GetComponent<CharacterController>();
    }

    void Update()
    {
        // Comprobar si está en el suelo
        isGrounded = charController.isGrounded;
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

        if (!isDashing)
        {
            HandleMovementInput();
            HandleJumpInput();
        }

        HandleDashInput();
        HandleSliceInput();
        HandleCrouchInput();
        HandleInteractInput();

        ApplyGravity();
        charController.Move(velocity * Time.deltaTime);
    }

    private void HandleMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // --- NUEVO: Suavizar el giro del sprite ---
        if (spriteTransform != null && horizontal != 0)
        {
            float yRot = horizontal > 0 ? 180f : 0f;
            targetSpriteRotation = Quaternion.Euler(90f, yRot, 0f);
        }
        if (spriteTransform != null)
        {
            // Suaviza la rotación hacia el objetivo
            spriteTransform.localRotation = Quaternion.Slerp(
                spriteTransform.localRotation,
                targetSpriteRotation,
                10f * Time.deltaTime // Ajusta la velocidad aquí
            );
        }
        // --- FIN NUEVO ---

        // Detectar doble toque para correr
        DetectDoubleTapForRun();

        float currentSpeed = isRunning ? runSpeed : speed;

        if (isGrounded)
        {
            if (direction.magnitude >= 0.1f)
            {
                Vector3 moveDir = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * direction; // Dirección del movimiento basada en la cámara
                charController.Move(moveDir.normalized * currentSpeed * Time.deltaTime); // Mover al jugador

                // Solo asigna momentum si hay input significativo
                airMomentum = moveDir.normalized * currentSpeed;
            }
            else
            {
                isRunning = false; // Detener carrera si no hay input

                // Reducir gradualmente el momentum en el suelo si no hay input
                airMomentum = Vector3.Lerp(airMomentum, Vector3.zero, 20f * Time.deltaTime);
                if (airMomentum.magnitude < 0.05f)
                    airMomentum = Vector3.zero;
            }
        }
        else // En el aire
        {
            // Si el jugador da input, suaviza el cambio de momentum
            if (direction.magnitude >= 0.1f)
            {
                Vector3 moveDir = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * direction;
                airMomentum = Vector3.Lerp(airMomentum, moveDir.normalized * currentSpeed, airControlLerp * Time.deltaTime);
            }
            // Aplica el momentum guardado
            charController.Move(new Vector3(airMomentum.x, 0f, airMomentum.z) * Time.deltaTime);
        }
    }

    private void HandleJumpInput()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || coyoteTimeCounter > 0f)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                coyoteTimeCounter = 0f;
                canDoubleJump = true; // Permitir doble salto solo después de un salto válido
            }
            else if (canDoubleJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                canDoubleJump = false;
            }
        }
    }

    // DASH en E (ya implementado)
    private void HandleDashInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetKeyDown(KeyCode.E) && direction.magnitude >= 0.1f && !isDashing)
        {
            StartCoroutine(DashCoroutine(direction));
        }
    }

    // SLICE en Shift
    private void HandleSliceInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            // Ejecutar acción de slice/ataque rápido
            Debug.Log("Slice ejecutado");
            // Aquí va tu lógica de ataque rápido
        }
    }

    // CROUCH en Ctrl
    private void HandleCrouchInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl))
        {
            // Ejecutar acción de agacharse
            Debug.Log("Crouch iniciado");
            // Aquí va tu lógica de agacharse
        }
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl))
        {
            // Ejecutar acción de dejar de agacharse
            Debug.Log("Crouch terminado");
            // Aquí va tu lógica para volver a la posición normal
        }
    }

    // INTERACT en F
    private void HandleInteractInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Ejecutar acción de interacción
            Debug.Log("Interacción ejecutada");
            // Aquí va tu lógica de interacción
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
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
        // Detectar doble toque para correr
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

    public void RespawnAtCheckpoint()
    {
        if (lastCheckpoint != null)
        {
            transform.position = lastCheckpoint.transform.position; // Reiniciar posición al último checkpoint
            // Resetear estado si es necesario
        }
        else
        {
            // Reiniciar nivel o lógica alternativa
        }
    }
}