// NotaCounterUI.cs
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class NotaCounterUI : MonoBehaviour
{
    public Inventory inventory; // Asigna el Inventory en el inspector
    public TMP_Text notasText;  // Asigna el TextMeshPro en el inspector

    public GameObject panelNota;      // Panel general de la nota (Canvas hijo)
    public RawImage rawImage;         // Para mostrar video o imagen
    public VideoPlayer videoPlayer;   // VideoPlayer (Render Mode: RenderTexture)
    public Image imagenNota;          // Para mostrar Sprite si es imagen

    private Coroutine hideCoroutine;

    void Update()
    {
        int count = 0;
        foreach (var item in inventory.collectibles)
        {
            if (item.itemData != null && item.itemData.esNota)
                count++;
        }
        notasText.text = count.ToString();
    }

    public void MostrarNota(InventoryItemData itemData)
    {
        if (panelNota != null)
            panelNota.SetActive(true);

        if (itemData.notaVideo != null)
        {
            if (imagenNota != null) imagenNota.gameObject.SetActive(false);
            if (rawImage != null) rawImage.gameObject.SetActive(true);

            if (videoPlayer != null)
            {
                videoPlayer.gameObject.SetActive(true);
                videoPlayer.clip = itemData.notaVideo;
                videoPlayer.Play();
                videoPlayer.loopPointReached += OcultarNotaAlTerminarVideo;
            }
        }
        // Mostrar imagen si existe
        else if (itemData.notaImagen != null)
        {
            Debug.Log("Mostrando imagen: " + itemData.notaImagen.name);
            if (rawImage != null) rawImage.gameObject.SetActive(false);
            if (imagenNota != null)
            {
                imagenNota.gameObject.SetActive(true);
                imagenNota.sprite = itemData.notaImagen;
            }
            if (videoPlayer != null) videoPlayer.gameObject.SetActive(false);

            // Ocultar después de un tiempo
            if (hideCoroutine != null) StopCoroutine(hideCoroutine);
            hideCoroutine = StartCoroutine(OcultarNotaTrasTiempo(itemData.imagenMostrarTiempo));
        }
        // Si no hay nada, ocultar
        else
        {
            Debug.Log("No hay imagen ni video en este itemData: " + itemData.name);
            OcultarNota();
        }
    }

    private void OcultarNotaAlTerminarVideo(VideoPlayer vp)
    {
        videoPlayer.loopPointReached -= OcultarNotaAlTerminarVideo;
        OcultarNota();
    }

    private IEnumerator OcultarNotaTrasTiempo(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        OcultarNota();
    }

    public void OcultarNota()
    {
        if (panelNota != null)
            panelNota.SetActive(false);
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.gameObject.SetActive(false);
        }
        if (imagenNota != null)
            imagenNota.gameObject.SetActive(false);
    }
}