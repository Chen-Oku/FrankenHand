using UnityEngine;
using UnityEngine.SceneManagement;

public class VidaPlayer : SistemaVida
{
    public AudioSource audioSource;
    public AudioClip[] sonidosDanio; // Puedes asignar varios sonidos en el inspector

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        OnMuerte += Respawn;
        // Asegúrate de que la GUI se actualice después de inicializar la vida
        Object.FindFirstObjectByType<VidaGUIProbeta>()?.ActualizarProbeta();
    }

    public override void RecibirDanio(int cantidad)
    {
        base.RecibirDanio(cantidad);
        ReproducirSonidoDanio();
    }

    private void ReproducirSonidoDanio()
    {
        if (audioSource != null && sonidosDanio != null && sonidosDanio.Length > 0)
        {
            int idx = Random.Range(0, sonidosDanio.Length);
            audioSource.PlayOneShot(sonidosDanio[idx]);
        }
    }

    private void Respawn()
    {
        // Si ya no tiene vidas, ir a Game Over
        if (vidaActual <= 0)
        {
            OnGameOver();
            return;
        }

        // Llama a tu lógica de respawn
        GetComponent<PlayerController>()?.RespawnAtCheckpoint();
        // Opcional: restaurar vida
        vidaActual = maxContenedores * vidaPorContenedor;
        RaiseOnVidaCambiada();
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
