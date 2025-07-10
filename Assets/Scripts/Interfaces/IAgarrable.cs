using UnityEngine;

public interface IAgarrable
{
    void Agarrar(Transform jugador);
    void Soltar();
    void Arrastrar(Vector3 destino);
}