using UnityEngine;
using System;

public class SistemaVida : MonoBehaviour
{
    public int maxContenedores = 4;
    public int vidaPorContenedor = 2;
    public int vidaActual;

    public event Action OnVidaCambiada;
    public event Action OnMuerte;

    protected virtual void Start()
    {
        vidaActual = maxContenedores * vidaPorContenedor;
    }

    public virtual void RecibirDanio(int cantidad)
    {
        vidaActual -= cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, maxContenedores * vidaPorContenedor);
        RaiseOnVidaCambiada();

        if (vidaActual <= 0)
            OnMuerte?.Invoke();
    }

    public virtual void Curar(int cantidad)
    {
        vidaActual += cantidad;
        vidaActual = Mathf.Clamp(vidaActual, 0, maxContenedores * vidaPorContenedor);
        RaiseOnVidaCambiada();
    }

    protected void RaiseOnVidaCambiada()
    {
        OnVidaCambiada?.Invoke();
    }
}
