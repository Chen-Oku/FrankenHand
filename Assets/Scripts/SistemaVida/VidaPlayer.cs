using UnityEngine;
using UnityEngine.SceneManagement;

public class VidaPlayer : SistemaVida
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        OnMuerte += Respawn;
        // Asegúrate de que la GUI se actualice después de inicializar la vida
        Object.FindFirstObjectByType<VidaGUI>()?.ActualizarGUI();
    }

    private void Respawn()
    {
        // Llama a tu lógica de respawn
        GetComponent<PlayerController>()?.RespawnAtCheckpoint();
        // Opcional: restaurar vida
        vidaActual = maxContenedores * vidaPorContenedor;
        RaiseOnVidaCambiada(); // <--- Usa el método protegido
    }

        private void OnGameOver()
    {
        // Cargar la escena de Game Over
        SceneManager.LoadScene("GameOver");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
