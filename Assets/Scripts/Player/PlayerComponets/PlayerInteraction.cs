using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private DraggableObject1 objetoArrastrado;
    private IAgarrable objetoAgarrado;
    private Animator animator;
    private bool estabaEmpujando = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        HandleInteractInput();

        // Animaciones de empuje/agarrar
        bool estaAgarrando = objetoArrastrado != null;
        bool estaEmpujando = false;

        if (estaAgarrando)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            float pushSpeed = new Vector2(horizontal, vertical).magnitude;
            animator.SetFloat("pushSpeed", pushSpeed);

            float hRaw = Input.GetAxisRaw("Horizontal");
            float vRaw = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(hRaw, 0f, vRaw);

            estaEmpujando = direction.magnitude > 0.1f;

            if (!estabaEmpujando && estaEmpujando)
                animator.SetTrigger("startPush");

            animator.SetBool("isPushing", estaEmpujando);
            animator.SetBool("isHoldingPush", !estaEmpujando);

            // --- Mueve el objeto manualmente ---
            if (estaEmpujando && objetoArrastrado != null)
            {
                // Obtén la referencia a la cámara principal
                Transform cam = Camera.main.transform;

                // Calcula la dirección de movimiento en el plano XZ, relativa a la cámara
                Vector3 inputDir = new Vector3(hRaw, 0f, vRaw).normalized;
                Vector3 moveDir = Quaternion.Euler(0f, cam.eulerAngles.y, 0f) * inputDir;

                // Mueve el objeto en esa dirección
                objetoArrastrado.transform.position += moveDir * pushSpeed * Time.deltaTime * 2f;
            }

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

    private void HandleInteractInput()
    {
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
}