using UnityEngine;
using UnityEngine.SceneManagement;

public class VidaPlayer : SistemaVida
{
<<<<<<< Updated upstream
=======
    private PlayerSoundController soundController;

>>>>>>> Stashed changes
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start()
    {
        base.Start();
        OnMuerte += Respawn;
        // Asegúrate de que la GUI se actualice después de inicializar la vida
<<<<<<< Updated upstream
        Object.FindFirstObjectByType<VidaGUI>()?.ActualizarGUI();
=======
        Object.FindFirstObjectByType<VidaGUIProbeta>()?.ActualizarProbeta();
    }

    void Awake()
    {
        soundController = GetComponent<PlayerSoundController>();
    }

    public override void RecibirDanio(int cantidad)
    {
        base.RecibirDanio(cantidad);
        soundController?.PlayManoLastimada();
>>>>>>> Stashed changes
    }

    private void Respawn()
    {
        // Llama a tu lógica de respawn
        var playerController = GetComponent<PlayerController>();
        if (playerController != null && playerController.respawn != null)
        {
            playerController.respawn.RespawnAtCheckpoint();
        }
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
