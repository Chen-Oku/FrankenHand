using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ShadowController : MonoBehaviour
{
    [SerializeField]float castDistance;
    DecalProjector projector;
    private void Start()
    {
        projector = GetComponent<DecalProjector>();
    }
    void Update()
    {
        if (Physics.Raycast(transform.position + (Vector3.up * .2f), Vector3.down, out RaycastHit hit, castDistance))
        {
            float distance = hit.distance;
            //Set the size to keep the shadow from showing on multiple surfaces
            projector.size = new Vector3(projector.size.x, projector.size.y, distance);
            //Fade the shadow as you get closer
            projector.fadeFactor = 1 - (distance / castDistance);
            //move the projecter to account for the new size
            projector.pivot = (Vector3.forward * (distance / 2 + -.1f));

            // CORREGIDO: Alinea el eje Y del projector con la normal de la superficie
            // Mantén la rotación base en X=90 y alinea con la normal
            Quaternion baseRot = Quaternion.Euler(90, 0, 0);
            Quaternion alignNormal = Quaternion.FromToRotation(Vector3.up, hit.normal);
            projector.transform.rotation = alignNormal * baseRot;
        }
    }

}