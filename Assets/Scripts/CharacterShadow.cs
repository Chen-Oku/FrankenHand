using UnityEngine;

public class CharacterShadow : MonoBehaviour
{
    public GameObject shadowObject; // Asigna el objeto de la sombra en el Inspector
    public float groundY = 0.0f; // Altura del piso donde se proyecta la sombra

    void Update() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f)) {
            shadowObject.transform.position = hit.point;
        } else {
            // Si no hay piso debajo, usa altura fija
            shadowObject.transform.position = new Vector3(
                transform.position.x,
                groundY,
                transform.position.z
            );
        }
    }
}
