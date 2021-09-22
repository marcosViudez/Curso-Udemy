using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float speedForce;
    private Rigidbody _rigidbody;
    private GameObject origin;

    // variables de los powerUps
    public bool hasPowerUp;
    public float powerUpForce;
    public float powerUpTime;
    public GameObject[] powerUpsIndicators;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        origin = GameObject.Find("Origin");
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");

        // la fuerza no depende del tiempo por eso no se pone time.deltatime
        // forceMode.force depende del peso de la bola
        _rigidbody.AddForce(origin.transform.forward * speedForce * forwardInput, ForceMode.Force);

        // posicion de los indicadores siguen al player
        foreach(GameObject indicators in powerUpsIndicators)
        {
            indicators.transform.position = this.transform.position + 0.6f * Vector3.down;
        }

        // si muere el player cargamos el juego de nuevo
        // o hacerlo por un triggerEnter
        if(this.transform.position.y < -10)
        {
            SceneManager.LoadScene("Prototype 4");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PowerUp"))
        {
            hasPowerUp = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountdown());
        }
    }

    // fin de la vida del powerUp
    IEnumerator PowerUpCountdown()
    {
        foreach(GameObject indicator in powerUpsIndicators)
        { 
            indicator.gameObject.SetActive(true);
            yield return new WaitForSeconds(powerUpTime / powerUpsIndicators.Length);
            indicator.gameObject.SetActive(false);
        }

        hasPowerUp = false;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerUp)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            // como las distancias es siempre la misma no se normaliza el vector
            Vector3 awakeFromPlayer = collision.gameObject.transform.position - this.transform.position;

            // impulse para fuerza inmediatas
            enemyRigidbody.AddForce(awakeFromPlayer * powerUpForce, ForceMode.Impulse);
        }
    }
}
