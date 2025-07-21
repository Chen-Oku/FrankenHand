using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public DraggableObject1 objetoArrastrado { get; set; }
    private IAgarrable objetoAgarrado;
    private Animator animator;
    private bool estabaEmpujando = false;

    public KeyCode interactKey = KeyCode.F;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Debug.Log("PlayerInteraction activo");
        HandleInteractInput();
        UpdatePlayerControlState();
    }

    private void UpdatePlayerControlState()
    {
        // Solo animar si el objeto agarrado es arrastrable
        bool estaAgarrando = objetoArrastrado != null && objetoArrastrado is IArrastrable;
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
        if (Input.GetKeyDown(interactKey))
        {
            // Si ya tienes un objeto agarrado, suéltalo
            if (objetoArrastrado != null)
            {
                objetoArrastrado.Soltar();
                objetoArrastrado = null;
            }
            else
            {
                // Si no tienes ninguno, busca uno cercano e intenta agarrarlo
                IInteractuable interactuable = DetectarInteractuableCercano();
                if (interactuable != null)
                    interactuable.Interactuar();
            }
        }
    }

    private IInteractuable DetectarInteractuableCercano()
    {
        float radio = 1.5f;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radio);
        foreach (var col in colliders)
        {
            IInteractuable interactuable = col.GetComponent<IInteractuable>();
            if (interactuable != null)
                return interactuable;
        }
        return null;
    }

    public void AgarrarObjeto(DraggableObject1 objeto)
    {
        if (objetoArrastrado == null)
        {
            objetoArrastrado = objeto;
            objeto.Agarrar(transform);
        }
    }
}