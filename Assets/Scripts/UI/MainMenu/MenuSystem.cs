using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Play()
    {
        // Load the game scene
        SceneManager.LoadScene("Level1");
        Debug.Log("loading Level1");
    }

    public void Quit()
    {
        // Quit the application
        Application.Quit();
        
        // If running in the editor, stop playing
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
