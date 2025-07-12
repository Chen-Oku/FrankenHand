using UnityEngine;

public class TransparencyOnObstruct : MonoBehaviour
{
    public Transform player;
    public Camera mainCamera;
    public string obstructTag = "Obstruct";
    public float transparentAlpha = 0.3f;

    private Renderer lastRenderer;
    private Material lastMaterial;
    private Color originalColor;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        Vector3 dir = player.position - mainCamera.transform.position;
        Ray ray = new Ray(mainCamera.transform.position, dir.normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, dir.magnitude))
        {
            if (hit.collider.CompareTag(obstructTag))
            {
                Renderer rend = hit.collider.GetComponent<Renderer>();
                if (rend != null)
                {
                    if (lastRenderer != rend)
                    {
                        RestoreLast();
                        lastRenderer = rend;
                        lastMaterial = rend.material;
                        originalColor = lastMaterial.color;
                        Color c = lastMaterial.color;
                        c.a = transparentAlpha;
                        lastMaterial.color = c;
                        lastMaterial.SetFloat("_Surface", 1); // Para URP Standard Shader
                        lastMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                        lastMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                        lastMaterial.SetInt("_ZWrite", 0);
                        lastMaterial.DisableKeyword("_ALPHATEST_ON");
                        lastMaterial.EnableKeyword("_ALPHABLEND_ON");
                        lastMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                        lastMaterial.renderQueue = 3000;
                    }
                }
                return;
            }
        }
        RestoreLast();
    }

    void RestoreLast()
    {
        if (lastMaterial != null)
        {
            lastMaterial.color = originalColor;
            lastMaterial.SetFloat("_Surface", 0);
            lastMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            lastMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            lastMaterial.SetInt("_ZWrite", 1);
            lastMaterial.DisableKeyword("_ALPHABLEND_ON");
            lastMaterial.renderQueue = -1;
            lastMaterial = null;
            lastRenderer = null;
        }
    }
}