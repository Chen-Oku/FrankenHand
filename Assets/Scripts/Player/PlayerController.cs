using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController charController;
    public Transform cameraTransform;
    public Transform spriteVisualTransform; // Asigna el hijo visual en el Inspector
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

    private PauseMenu pauseMenu;

    private Quaternion targetSpriteRotation = Quaternion.Euler(90f, 180f, 0f); // Agrega este campo arriba
    private int lastFacing = 1; // 1 = derecha, -1 = izquierda

    private Animator animator; // Asigna el Animator en el Inspector si lo tienes

    private Rigidbody rb; // Para animaciones de caída

    private DraggableObject1 objetoArrastrado;
    private IAgarrable objetoAgarrado;

    private bool estabaEmpujando = false;

    void Start()
    {
        if (charController == null)
            charController = GetComponent<CharacterController>();
        if (animator == null)
            animator = GetComponent<Animator>();
        pauseMenu = Object.FindFirstObjectByType<PauseMenu>();

        // Aparecer en el spawn inicial
        PlayerSpawnPoint spawn = Object.FindFirstObjectByType<PlayerSpawnPoint>();
        if (spawn != null)
        {
            transform.position = spawn.transform.position;
            lastCheckpoint = null; // No hay checkpoint aún
        }
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

        // --- Billboarding: el objeto principal mira a la cámara (solo Y) ---
        if (cameraTransform != null)
        {
            Vector3 toCamera = cameraTransform.position - transform.position;
            toCamera.y = 0f;
            if (toCamera.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(toCamera.normalized, Vector3.up);
                transform.rotation = lookRotation;
            }
        }
        
        animator.SetFloat("yVelocity", velocity.y); // Actualizar animación de caída

        // al caer por debajo de un cierto nivel, reiniciar al último checkpoint
        /*         if (transform.position.y < -10f && lastCheckpoint != null) // Ajusta el valor según tu escena
                {
                    transform.position = lastCheckpoint.transform.position;
                } */

        // Detectar si el jugador mantiene presionada la tecla F
        if (Input.GetKey(KeyCode.F))
        {
            if (objetoArrastrado == null)
            {
                IAgarrable agarrable = DetectarAgarrableCercano();
                if (agarrable != null)
                {
                    agarrable.Agarrar(transform);
                    objetoArrastrado = agarrable as DraggableObject1;
                }
            }
        }
        else
        {
            if (objetoArrastrado != null)
            {
                objetoArrastrado.Soltar();
                objetoArrastrado = null;
            }
        }

        // Detectar si el jugador mantiene presionada la tecla F para agarrar
        bool estaAgarrando = objetoArrastrado != null;

        // Detectar si está moviendo el objeto (por ejemplo, si hay input de movimiento y está agarrando)
        bool estaEmpujando = false;
        if (estaAgarrando)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float pushSpeed = new Vector2(horizontal, vertical).magnitude;
            animator.SetFloat("pushSpeed", pushSpeed);
        }
        else
        {
            animator.SetFloat("pushSpeed", 0f);
        }

        if (estaAgarrando)
        {
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(horizontal, 0f, vertical);

            estaEmpujando = direction.magnitude > 0.1f;

            // Animación de inicio de empuje
            if (!estabaEmpujando && estaEmpujando)
            {
                animator.SetTrigger("startPush");
            }

            // Animación de empujando
            animator.SetBool("isPushing", estaEmpujando);

            // Animación de preparado (sosteniendo pero sin mover)
            animator.SetBool("isHoldingPush", !estaEmpujando);

            estabaEmpujando = estaEmpujando;
        }
        else
        {
            // Si suelta el objeto, desactiva animaciones de empuje
            animator.SetBool("isPushing", false);
            animator.SetBool("isHoldingPush", false);
            estabaEmpujando = false;
        }
    }

        private void OnTriggerEnter(Collider other)
    {
        Checkpoint checkpoint = other.GetComponent<Checkpoint>();
        if (checkpoint != null)
        {
            lastCheckpoint = checkpoint;
        }
    }


    private void HandleMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float moveAmount = new Vector2(horizontal, vertical).magnitude;
        float animSpeed = 0f;
        if (moveAmount > 0)
            animSpeed = isRunning ? 1f : 0.5f;
        animator.SetFloat("idleWaling", animSpeed);

        // Detectar dirección y preparar rotación tipo Paper Mario
        if (horizontal > 0.01f)
            lastFacing = 1;
        else if (horizontal < -0.01f)
            lastFacing = -1;

        // Flip Paper Mario: giro suave en Y local del hijo visual
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


        // Detectar doble toque para correr
        DetectDoubleTapForRun();

        float currentSpeed = isRunning ? runSpeed : speed;

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
        else // En el aire
        {
            if (direction.magnitude >= 0.1f)
            {
                Vector3 moveDir = Quaternion.Euler(0f, cameraTransform.eulerAngles.y, 0f) * direction;
                airMomentum = Vector3.Lerp(airMomentum, moveDir.normalized * currentSpeed, airControlLerp * Time.deltaTime);
            }
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
                animator.SetBool("isJumping", true); // Activar animación de salto
            }
            else if (canDoubleJump)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                canDoubleJump = false;
                animator.SetBool("isJumping", true); // Activar animación de salto
            }
        }
 
        if (isGrounded && velocity.y < 0)
        {
            animator.SetBool("isJumping", false); // Desactiva salto
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
            IAgarrable agarrable = DetectarAgarrableCercano();
            if (agarrable != null)
            {
                if (objetoAgarrado == null)
                {
                    agarrable.Agarrar(transform);
                    objetoAgarrado = agarrable as DraggableObject;
                }
                else
                {
                    objetoAgarrado.Soltar();
                    objetoAgarrado = null;
                }
            }
        }
    }

    private IAgarrable DetectarAgarrableCercano()
    {
        float radio = 1.5f;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radio);
        foreach (var col in colliders)
        {
            IAgarrable agarrable = col.GetComponent<IAgarrable>();
            if (agarrable != null)
                return agarrable;
        }
        return null;
    }


    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    private IEnumerator DashCoroutine(Vector3 direction)
    {
        isDashing = true;
        animator.SetBool("isDashing", true); // Activa animación de dash
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
        animator.SetBool("isDashing", false); // Vuelve a animación normal
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
            if (charController != null)
                charController.enabled = false;

            transform.position = lastCheckpoint.transform.position;

            if (charController != null)
                charController.enabled = true;

            Debug.Log("Reaparecido en el checkpoint: " + lastCheckpoint.name);
        }
        else
        {
            PlayerSpawnPoint spawn = Object.FindFirstObjectByType<PlayerSpawnPoint>();
            if (spawn != null)
            {
                if (charController != null)
                    charController.enabled = false;

                transform.position = spawn.transform.position;

                if (charController != null)
                    charController.enabled = true;
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rb = hit.collider.attachedRigidbody;
        if (rb != null && !rb.isKinematic)
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            rb.AddForce(pushDir * 5f, ForceMode.Impulse); // Ajusta la fuerza según lo necesites
        }
    }

    public Vector3 Velocity
    {
        get => velocity;
        set => velocity = value;
    }
}