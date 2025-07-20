using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class NoteUIManager : MonoBehaviour
{
    public GameObject notePanel;
    public VideoPlayer videoPlayer;

    public static NoteUIManager Instance;

    void Awake()
    {
        Instance = this;
        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoEnd;
    }

    public void ShowNote(VideoClip video)
    {
        notePanel.SetActive(true);
        if (videoPlayer != null && video != null)
        {
            videoPlayer.clip = video;
            videoPlayer.Play();
        }
    }

    public void HideNote()
    {
        if (videoPlayer != null)
            videoPlayer.Stop();
        notePanel.SetActive(false);
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        HideNote();
    }
}