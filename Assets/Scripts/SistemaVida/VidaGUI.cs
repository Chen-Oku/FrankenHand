using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class VidaGUI : MonoBehaviour
{
    public VidaPlayer vidaPlayer;
    public GameObject corazonPrefab; // Prefab del corazón (Image)
    public Transform contenedorCorazones; // Panel o contenedor en el Canvas
    public Sprite corazonLleno;
    public Sprite corazonMedio; 
    public Sprite corazonVacio;

    private List<Image> corazones = new List<Image>();

    void Start()
    {
        InstanciarCorazones();
        vidaPlayer.OnVidaCambiada += ActualizarGUI;
        StartCoroutine(ActualizarDespuesDeUnFrame());
    }

    private System.Collections.IEnumerator ActualizarDespuesDeUnFrame()
    {
        yield return null;
        ActualizarGUI();
    }

    void InstanciarCorazones()
    {
        // Limpia corazones previos
        foreach (Transform child in contenedorCorazones)
            Destroy(child.gameObject);
        corazones.Clear();

        int maxContenedores = vidaPlayer.maxContenedores;
        for (int i = 0; i < maxContenedores; i++)
        {
            GameObject corazonGO = Instantiate(corazonPrefab, contenedorCorazones);
            corazonGO.SetActive(true); // Por si el prefab está desactivado
            Image img = corazonGO.GetComponent<Image>();
            corazones.Add(img);
        }
    }

    public void ActualizarGUI()
    {
        int vida = vidaPlayer.vidaActual;
        int vidaPorCorazon = vidaPlayer.vidaPorContenedor;
        int total = corazones.Count;

        for (int i = 0; i < total; i++)
        {
            int heartIndex = total - 1 - i; // Para vaciar de derecha a izquierda
            int vidaEnEsteCorazon = Mathf.Clamp(vida - (i * vidaPorCorazon), 0, vidaPorCorazon);

            if (vidaEnEsteCorazon == vidaPorCorazon)
            {
                corazones[heartIndex].sprite = corazonLleno;
            }
            else if (vidaEnEsteCorazon == vidaPorCorazon / 2 && corazonMedio != null && vidaPorCorazon > 1)
            {
                corazones[heartIndex].sprite = corazonMedio;
            }
            else
            {
                corazones[heartIndex].sprite = corazonVacio;
            }
        }
    }
}