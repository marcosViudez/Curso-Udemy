using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    public GameObject boosterObject;
    private Rigidbody playerRb;
    private float speed = 5;
    private GameObject focalPoint;
    public GameObject FocalPoint { get => focalPoint; set => focalPoint = value; }

    private bool hasPowerup;
    public bool HasPowerup { get => hasPowerup; set => hasPowerup = value; }
    

    public GameObject[] powerupIndicator;
    public int powerUpDuration = 15;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup
    private float powerupBooster = 35;


    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        FocalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");
        playerRb.AddForce(FocalPoint.transform.forward * verticalInput * speed, ForceMode.Force);

        // Set powerup indicator position to beneath player
        // powerupIndicator.transform.position = transform.position + new Vector3(0, -0.6f, 0);
        // posicion de los indicadores siguen al player
        foreach (GameObject indicators in powerupIndicator)
        {
            indicators.transform.position = this.transform.position + 0.6f * Vector3.down;
        }

        // al pulsar la tecla space empujamos la pelota enemy con un booster
        bool isCollision = boosterObject.GetComponent<boosterCollision>().isCollision;

        if (Input.GetKeyDown(KeyCode.Space) && isCollision)
        {
            // Debug.Log("booster");

            GameObject targetEnemy = boosterObject.GetComponent<boosterCollision>().Target;
            Rigidbody target = targetEnemy.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = targetEnemy.gameObject.transform.position - this.transform.position;

            // activamos particulas de booster al golpear al enemigo
            target.GetComponent<EnemyX>().particlesEnemy.Play();
            // cambiamos el color de las particulas
            target.GetComponent<EnemyX>().particlesEnemy.startColor = Color.red;

            // booster fuerza
            target.AddForce(awayFromPlayer * powerupBooster, ForceMode.Impulse);
        }
    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            HasPowerup = true;
            other.gameObject.GetComponent<ParticleActivePowerUp>().runActivatePowerUp();
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        foreach(GameObject indicator in powerupIndicator)
        {
            indicator.gameObject.SetActive(true);
            yield return new WaitForSeconds(powerUpDuration / powerupIndicator.Length);
            HasPowerup = false;
            indicator.gameObject.SetActive(false);
        }
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - this.transform.position; 
           
            if (HasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }
        }

    }
}
