using UnityEngine;
using UnityEngine.SceneManagement;

public class VidaPlayer : SistemaVida
{
    private PlayerSoundController soundController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        soundController = GetComponent<PlayerSoundController>();
        OnMuerte += Respawn;
        // Asegúrate de que la GUI se actualice después de inicializar la vida
        Object.FindFirstObjectByType<VidaGUIProbeta>()?.ActualizarProbeta();
    }

    public override void RecibirDanio(int cantidad)
    {
        base.RecibirDanio(cantidad);
        soundController?.PlayManoLastimada();

        // Retroceso y parpadeo
        var pc = GetComponent<PlayerController>();
        if (pc != null)
        {
            // Puedes ajustar la dirección, fuerza y duración según el contexto
            Vector3 knockbackDir = -transform.forward; // Por defecto, hacia atrás
            pc.ApplyKnockback(knockbackDir);
            pc.FlashRed(0.5f, 3);
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
