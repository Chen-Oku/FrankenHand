using UnityEngine;
using System.Collections.Generic; // Asegúrate de incluir esto para usar List<>

public class PlayerSoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sonidoSalto;
    public AudioClip sonidoGatoPrimeraAparicion;
    public AudioClip sonidoCauldron;
    public AudioClip sonidoManoAPuntoDeCaer;
    public AudioClip sonidoManoCaminando;
    public AudioClip sonidoDeGolpeCaidaMano;
    public AudioClip[] sonidosManoLastimada;
    public AudioClip sonidoSalpicaduraPinturaVerde;
    public AudioClip sonidoSalpicaduraPinturaAzul;
    public AudioClip sonidoSalpicaduraPinturaVerdePequeña;
    public AudioClip sonidoPapel;
    public AudioClip sonidoRecogerElementos;
    public AudioClip sonidoPanelDeRocaAscendente;
    public AudioClip sonidoBofetadaGato1;
    public AudioClip sonidoBofetadaGato2;
    public AudioClip sonidoBofetadaGato3;
    public AudioClip[] sonidosSalpicaduraCharco;

    private float lastStepTime = -1f;
    private float stepCooldown = 2.0f; // Ahora coincide con la duración del audio de caminar

    public void PlaySaltar()
    {
        audioSource.PlayOneShot(sonidoSalto);
    }

    public void PlayGatoPrimeraAparicion()
    {
        audioSource.PlayOneShot(sonidoGatoPrimeraAparicion);
    }

    public void PlayCauldron()
    {
        audioSource.PlayOneShot(sonidoCauldron);
    }

    public void PlayManoAPuntoDeCaer()
    {
        audioSource.PlayOneShot(sonidoManoAPuntoDeCaer);
    }

    public void PlayManoCaminando()
    {
        if (audioSource == null || sonidoManoCaminando == null)
            return;

        if (Time.time - lastStepTime > stepCooldown)
        {
            audioSource.PlayOneShot(sonidoManoCaminando);
            lastStepTime = Time.time;
        }
    }

    public void PlayDeGolpeCaidaMano()
    {
        audioSource.PlayOneShot(sonidoDeGolpeCaidaMano);
    }

    public void PlayManoLastimada()
    {
        if (sonidosManoLastimada != null && sonidosManoLastimada.Length > 0)
        {
            int idx = Random.Range(0, sonidosManoLastimada.Length);
            audioSource.PlayOneShot(sonidosManoLastimada[idx]);
        }
    }

    public void PlaySalpicaduraPinturaVerde()
    {
        audioSource.PlayOneShot(sonidoSalpicaduraPinturaVerde);
    }

    public void PlaySalpicaduraPinturaAzul()
    {
        audioSource.PlayOneShot(sonidoSalpicaduraPinturaAzul);
    }

    public void PlaySalpicaduraPinturaVerdePequeña()
    {
        audioSource.PlayOneShot(sonidoSalpicaduraPinturaVerdePequeña);
    }

    public void PlayPapel()
    {
        audioSource.PlayOneShot(sonidoPapel);
    }

    public void PlayRecogerElementos()
    {
        audioSource.PlayOneShot(sonidoRecogerElementos);
    }

    public void PlayPanelDeRocaAscendente()
    {
        audioSource.PlayOneShot(sonidoPanelDeRocaAscendente);
    }

    public void PlayBofetadaGato1()
    {
        audioSource.PlayOneShot(sonidoBofetadaGato1);
    }

    public void PlayBofetadaGato2()
    {
        audioSource.PlayOneShot(sonidoBofetadaGato2);
    }
    
    public void PlayBofetadaGato3()
    {
        audioSource.PlayOneShot(sonidoBofetadaGato3);
    }
     public void PlaySalpicaduraCharco()
    {
        if (sonidosSalpicaduraCharco != null && sonidosSalpicaduraCharco.Length > 0)
        {
            int idx = Random.Range(0, sonidosSalpicaduraCharco.Length);
            audioSource.PlayOneShot(sonidosSalpicaduraCharco[idx]);
        }
    }

    public void PlayBofetadaGatoRandom()
    {
        AudioClip[] bofetadas = new AudioClip[] { sonidoBofetadaGato1, sonidoBofetadaGato2, sonidoBofetadaGato3 };
        List<AudioClip> disponibles = new List<AudioClip>();
        foreach (var clip in bofetadas)
            if (clip != null) disponibles.Add(clip);

        if (disponibles.Count > 0)
            audioSource.PlayOneShot(disponibles[Random.Range(0, disponibles.Count)]);
    }
}
