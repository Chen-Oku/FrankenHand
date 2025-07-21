// NotaCounterUI.cs
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class NotaCounterUI : MonoBehaviour
{
    public Inventory inventory;
    public TMP_Text notasText;
    public GameObject panelNota;
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public Image imagenNota;

    private Coroutine hideCoroutine;
    private int totalNotas = 5;

    void Update()
    {
        int count = 0;
        foreach (var item in inventory.collectibles)
        {
            if (item.itemData != null && item.itemData.esNota)
                count++;
        }
        notasText.text = $"{count}/{totalNotas}";
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
        else if (itemData.notaImagen != null)
        {
            if (rawImage != null) rawImage.gameObject.SetActive(false);
            if (imagenNota != null)
            {
                imagenNota.gameObject.SetActive(true);
                imagenNota.sprite = itemData.notaImagen;
            }
            if (videoPlayer != null) videoPlayer.gameObject.SetActive(false);

            if (hideCoroutine != null) StopCoroutine(hideCoroutine);
            hideCoroutine = StartCoroutine(OcultarNotaTrasTiempo(itemData.imagenMostrarTiempo));
        }
        else
        {
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