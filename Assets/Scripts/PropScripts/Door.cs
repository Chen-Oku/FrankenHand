using UnityEngine;
using cakeslice;

public class Door : MonoBehaviour
{
    public Inventory inventory;
    public InventoryItemData requiredKeyData; // Asigna el ScriptableObject de la llave en el Inspector
    public float openDistance = 4f; // Distancia para interactuar
    public GameObject interactMessage; // Asigna el objeto de mensaje en el inspector

    private bool playerInRange = false;
    private Outline outline;
    private Transform playerTransform;

    void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline != null)
            outline.enabled = false;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerTransform = playerObj.transform;
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
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && outline != null)
        {
            outline.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && outline != null)
        {
            outline.enabled = false;
            playerInRange = false;
            if (interactMessage != null)
                interactMessage.SetActive(false);
        }
    }

    public void TryOpen()
    {
        if (inventory.RemoveItem(requiredKeyData, 1))
        {
            Debug.Log("Puerta abierta");
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
            col.enabled = false;
            Debug.Log("La puerta ha sido abierta.");
        }
        if (interactMessage != null)
            interactMessage.SetActive(false);
    }

    private System.Collections.IEnumerator OpenDoorAnimation()
    {
        float duration = 1f;
        float elapsed = 0f;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, 90f, 0);

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Lerp(startRot, endRot, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = endRot;
        WhenOpened();
        // Opcional: Destroy(gameObject); si quieres eliminar la puerta
    }
}
