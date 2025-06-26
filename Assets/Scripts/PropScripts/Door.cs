using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.UI;
using cakeslice; // Importante: agrega esto arriba

public class Door : MonoBehaviour
{
    public Inventory inventory;
    public InventoryItemData requiredKeyData; // Asigna el ScriptableObject de la llave en el Inspector
    public float openDistance = 4f; // Distancia para interactuar
    private float liftingDistance = 10f; // Distancia que se levantará la puerta al abrir
    public GameObject interactMessage; // Asigna el objeto de mensaje en el inspector
    //public Material normalMaterial;
    //public Material outlineMaterial; // Asigna el material de Shader Graph

    private MeshRenderer meshRenderer;
    private bool playerInRange = false;
    private OutlineEffect outline;

    void Awake()
    {
/*         meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null && outlineMaterial != null)
            meshRenderer.material = outlineMaterial; // Siempre usa el material con outline */

        outline = GetComponent<OutlineEffect>();
        if (outline != null)
            outline.enabled = false; // Desactiva el outline al inicio
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            TryOpen();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactMessage != null)
                interactMessage.SetActive(true);

            if (inventory == null)
                inventory = other.GetComponent<Inventory>();

/*             // Activa el outline
            if (outlineMaterial != null)
                outlineMaterial.SetFloat("Outline Thickness", 0.03f); // Ajusta el nombre según tu shader */
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactMessage != null)
                interactMessage.SetActive(false);

/*             // Desactiva el outline
            if (outlineMaterial != null)
                outlineMaterial.SetFloat("Outline Thickness", 0f); // Ajusta el nombre según tu shader */
        }
    }

    public void TryOpen()
    {
        if (inventory.RemoveItem(requiredKeyData, 1))
        {
            Debug.Log("Puerta abierta");
            // Ejemplo de animación simple: mover la puerta hacia arriba
            StartCoroutine(OpenDoorAnimation());
        }
        else
        {
            Debug.Log("Necesitas la llave para abrir esta puerta.");
        }
    }

    public void WhenOpened()
    {
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false; // Desactiva el collider trigger de la puerta
            Debug.Log("La puerta ha sido abierta.");
        }
        if (interactMessage != null)
            interactMessage.SetActive(false); // Oculta el mensaje si aún está activo
    }

    private System.Collections.IEnumerator OpenDoorAnimation()
    {
        float duration = 1f;
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + Vector3.up * liftingDistance; // Mueve la puerta 3 unidades hacia arriba

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
        WhenOpened();
        // Opcional: Destroy(gameObject); si quieres eliminar la puerta
    }
}
