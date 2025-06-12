using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class ShadowOnly : MonoBehaviour
{
    void Start()
    {
        var renderer = GetComponent<MeshRenderer>();
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
    }
}