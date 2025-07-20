using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void VolverAJugar()
    {
        // Recarga la escena actual del nivel
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); // O usa el nombre de la escena del nivel
    }

    public void IrAlMenuPrincipal()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SalirDelJuego()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
