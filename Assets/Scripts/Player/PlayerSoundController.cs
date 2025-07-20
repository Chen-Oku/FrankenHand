using UnityEngine;

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
        audioSource.PlayOneShot(sonidoManoCaminando);
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
}
