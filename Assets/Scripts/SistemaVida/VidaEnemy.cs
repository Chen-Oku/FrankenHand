using UnityEngine;

public class VidaEnemy : SistemaVida
{
    protected override void Start()
    {
        base.Start();
        OnMuerte += () => Destroy(gameObject);
    }
}