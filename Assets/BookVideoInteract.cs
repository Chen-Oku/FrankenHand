using UnityEngine;
using UnityEngine.Video;

public class BookVideoInteract : MonoBehaviour, IInteractuable
{
    public GameObject interactMessage;
    public GameObject videoUI;
    public GameObject videoPlayerObject;

    private VideoPlayer videoPlayer;
    private bool videoPlaying = false;
    private PlayerController playerController;

    void Awake()
    {
        if (interactMessage != null)
            interactMessage.SetActive(false);

        if (videoUI != null)
            videoUI.SetActive(false);

        if (videoPlayerObject != null)
            videoPlayer = videoPlayerObject.GetComponent<VideoPlayer>();

        playerController = FindFirstObjectByType<PlayerController>();
    }

    void Update()
    {
        // Solo dejar la lógica de ESC para saltar el video
        if (videoPlaying && Input.GetKeyDown(KeyCode.Escape))
        {
            StopVideo();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && interactMessage != null)
        {
            interactMessage.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && interactMessage != null)
        {
            interactMessage.SetActive(false);
        }
    }

    private void StartVideo()
    {
        videoPlaying = true;
        if (playerController != null)
            playerController.puedeMover = false;

        if (interactMessage != null)
            interactMessage.SetActive(false);

        if (videoUI != null)
            videoUI.SetActive(true);

        if (videoPlayerObject != null)
            videoPlayerObject.SetActive(true);

        if (videoPlayer != null)
        {
            videoPlayer.Play();
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    private void StopVideo()
    {
        videoPlaying = false;
        if (playerController != null)
            playerController.puedeMover = true;

        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
        if (videoUI != null)
            videoUI.SetActive(false);
        if (videoPlayerObject != null)
            videoPlayerObject.SetActive(false);
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        StopVideo();
    }

    // Interfaz de interacción
    public void Interactuar()
    {
        if (!videoPlaying)
            StartVideo();
    }
}
