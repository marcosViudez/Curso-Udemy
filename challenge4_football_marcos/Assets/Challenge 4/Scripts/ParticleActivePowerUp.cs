using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleActivePowerUp : MonoBehaviour
{
    public GameObject particlePowerUp;
    private float timePlayParticlesPowerUp = 1.0f;


    // activa nuestra Coroutine
    public void runActivatePowerUp()
    {
        StartCoroutine(OnParticlesPowerUps());
    }

    /// <summary>
    /// Activa el sistema de particulas del powerUp cuando lo coge el player
    /// </summary>
    IEnumerator OnParticlesPowerUps()
    {
        // hace al powerUp invisible
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;

        // activa las particulas del powerUp
        this.particlePowerUp.GetComponent<ParticleSystem>().Play();

        // esperamos unos segundos para que ejecute las particulas
        yield return new WaitForSeconds(this.timePlayParticlesPowerUp);

        // destruimos el powerUp
        Destroy(this.gameObject);
    }

}
