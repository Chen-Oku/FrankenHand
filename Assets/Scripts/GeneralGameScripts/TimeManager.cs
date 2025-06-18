using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    private int pauseRequests = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RequestPause()
    {
        pauseRequests++;
        Time.timeScale = 0f;
            Debug.Log("TimeManager: Juego reanudado en frame " + Time.frameCount);
    }

    public void RequestResume()
    {
        pauseRequests = Mathf.Max(0, pauseRequests - 1);
        if (pauseRequests == 0)
        {
            Time.timeScale = 1f;
        }
    }

    public bool IsPaused()
    {
        return Time.timeScale == 0f;
    }
}