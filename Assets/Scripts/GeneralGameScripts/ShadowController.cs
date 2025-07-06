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
        if (Physics.Raycast(transform.position+(Vector3.up*.2f), Vector3.down, out RaycastHit hit, castDistance)){
        float distance = hit.distance;
        //Set the size to keep the shadow from showing on multiple surfaces
        projector.size = new Vector3(projector.size.x,projector.size.y,distance);
        //Fade the shadow as you get closer
        projector.fadeFactor = 1-(distance/castDistance);
        //move the projecter to account for the new size
        projector.pivot = (Vector3.forward*(distance/2+-.1f));
        }
    }

}